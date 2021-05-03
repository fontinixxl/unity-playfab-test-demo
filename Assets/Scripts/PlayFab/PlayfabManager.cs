using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;
using PlayFab.PfEditor.Json;

public static class PlayfabManager 
{
    // Events to subscribe for this service
    public static event Action<bool> OnInventoryLoadEvent;

    // The user's Title specific DisplayName
    //public static string UserDisplayName = null;

    public static readonly Dictionary<string, int> virtualCurrency = new Dictionary<string, int>();
    public static int CoinsBalance => virtualCurrency[coinsCode];
    public static int LivesBalance => virtualCurrency[livesCode];
    public static int SecondsToRecharge = 0;

    // PLAYFAB DATA
    const string extraLivesBundleId = "extraLivesBundle";
    public const int livesBundlePrice = 10;
    const string coinsCode = "CO";
    const string livesCode = "LV";

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
        Debug.Log(JsonWrapper.SerializeObject(result.FunctionResult));
    }
}
