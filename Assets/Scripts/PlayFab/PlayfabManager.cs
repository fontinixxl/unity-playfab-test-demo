using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;

public static class PlayfabManager 
{
    // Events to subscribe for this service
    public static event Action<bool> OnInventoryLoadEvent;

    // Flag set after successfull PlayFab Login
    public static bool IsLoggedIn = false;
    // Flag set when the user's AccountInfo is loaded
    public static bool IsAccountInfoLoaded = false;

    // The user's Title specific DisplayName
    public static string UserDisplayName = null;

    public static readonly Dictionary<string, int> virtualCurrency = new Dictionary<string, int>();
    public static int CoinsBalance => virtualCurrency[coinsCode];
    public static int LivesBalance => virtualCurrency[livesCode];
    public static int SecondsToRecharge = 0;

    // PLAYFAB DATA
    const string extraLivesBundleId = "extraLivesBundle";
    public const int livesBundlePrice = 10;
    const string coinsCode = "CO";
    const string livesCode = "LV";

    public static void LoadAccountData()
    {
        PlayFabClientAPI.GetAccountInfo(
            // Request
            new GetAccountInfoRequest
            {
                // No properties means get the calling user's info
            },
            // Success
            (GetAccountInfoResult response) =>
            {
                Debug.Log("GetAccountInfo completed.");
                UserDisplayName = response.AccountInfo.TitleInfo.DisplayName;
                IsAccountInfoLoaded = true;
            },
            // Failure
            OnApiCallError
            );
    }

    ////////////////////////////////////////////////////////////////
    /// Update the user's per-title DisplayName
    /// 
    public static void SetUserDisplayName(string name)
    {
        UserDisplayName = name;

        PlayFabClientAPI.UpdateUserTitleDisplayName(
            // Request
            new UpdateUserTitleDisplayNameRequest
            {
                DisplayName = name
            },
            // Success
            (UpdateUserTitleDisplayNameResult result) =>
            {
                Debug.Log("UpdateUserTitleDisplayName completed.");
            },
            // Failure
            OnApiCallError
            );
    }

    ////////////////////////////////////////////////////////////////
    /// Update the user's game stats in bulk
    ///
    /// This uses a custom event to trigger cloudscript which
    /// performs the stat updates
    ///
    public static void UpdateStatistics(Dictionary<string, object> values)
    {
        PlayFabClientAPI.WritePlayerEvent(
            // Request
            new WriteClientPlayerEventRequest
            {
                EventName = "update_statistics",
                Body = new Dictionary<string, object>
                {
                    { "stats", values }
                }
            },
            // Success
            (WriteEventResponse response) =>
            {
                Debug.Log("WritePlayerEvent (UpdateStatistics) completed.");
            },
            // Failure
            OnApiCallError
            );
    }

    ////////////////////////////////////////////////////////////////
    /// Update a user's individual game stat
    ///
    /// This uses a custom event to trigger cloudscript which
    /// performs the stat updates
    ///
    public static void UpdateStatistic(string stat, int value)
    {
        PlayFabClientAPI.WritePlayerEvent(
            // Request
            new WriteClientPlayerEventRequest
            {
                EventName = "update_statistic",
                Body = new Dictionary<string, object>
                {
                    { "stat_name", stat },
                    { "value", value }
                }
            },
            // Success
            (WriteEventResponse response) =>
            {
                Debug.Log("WritePlayerEvent (UpdateStatistic) completed.");
            },
            // Failure
            OnApiCallError
            );
    }

    public static void GetInventory()
    {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), (GetUserInventoryResult result) =>
        {
            virtualCurrency.Clear();
            foreach (var pair in result.VirtualCurrency)
            {
                virtualCurrency.Add(pair.Key, pair.Value);
            }

            bool showCountDown = false;
            VirtualCurrencyRechargeTime rechargeDetails;
            if (result.VirtualCurrencyRechargeTimes.TryGetValue(livesCode, out rechargeDetails))
            {
                showCountDown = LivesBalance < rechargeDetails.RechargeMax;
                SecondsToRecharge = rechargeDetails.SecondsToRecharge;
            }
            OnInventoryLoadEvent?.Invoke(showCountDown);

        }, (error) =>
        {
            Debug.LogError(error.ToString());
        });
    }

    public static void SavePlayerData(int coinsCollected, bool gameOver = false)
    {
        var request = new ExecuteCloudScriptRequest
        {
            FunctionName = "SaveProgress",
            FunctionParameter = new { CoinsCollected = coinsCollected, IsGameOver = gameOver },
        };
        PlayFabClientAPI.ExecuteCloudScript(request, OnSavePlayerDataSuccess, OnApiCallError);
    }

    public static void TryBuyLives()
    {
        Debug.Log("Purchaseing Lives...");
        PurchaseItemRequest request = new PurchaseItemRequest() { ItemId = extraLivesBundleId, VirtualCurrency = coinsCode, Price = livesBundlePrice };
        PlayFabClientAPI.PurchaseItem(request, TryBuyLivesCallback, OnApiCallError);
    }

    static void TryBuyLivesCallback(PurchaseItemResult result)
    {
        Debug.Log("Lives Purchased!");
        GetInventory();
    }



    static void OnApiCallError(PlayFabError err)
    {
        string http = string.Format("HTTP:{0}", err.HttpCode);
        string message = string.Format("ERROR:{0} -- {1}", err.Error, err.ErrorMessage);
        string details = string.Empty;

        if (err.ErrorDetails != null)
        {
            foreach (var detail in err.ErrorDetails)
            {
                details += string.Format("{0} \n", detail.ToString());
            }
        }

        Debug.LogError(string.Format("{0}\n {1}\n {2}\n", http, message, details));
    }

    static void OnSavePlayerDataSuccess(ExecuteCloudScriptResult result)
    {
        Debug.Log("Player Data saved successfully");
    }
}
