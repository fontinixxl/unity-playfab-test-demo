using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;
using PlayFab.PfEditor.Json;

public static class PlayfabManager 
{
    const string coinsCode = "CO";
    const string livesCode = "LV";

    // Events to subscribe for this service
    public static event Action<bool> OnInventoryLoadEvent;

    // The user's Title specific DisplayName
    //public static string UserDisplayName = null;

    public static readonly Dictionary<string, int> virtualCurrency = new Dictionary<string, int>();
    public static int CoinsBalance => virtualCurrency[coinsCode];
    public static int LivesBalance => virtualCurrency[livesCode];
    public static int SecondsToRecharge = 0;

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
        PlayFabClientAPI.ExecuteCloudScript(request, OnSavePlayerDataSuccess, OnSavePlayerDataError);
    }

    private static void OnSavePlayerDataSuccess(ExecuteCloudScriptResult result)
    {
        Debug.Log(JsonWrapper.SerializeObject(result.FunctionResult));
    }

    private static void OnSavePlayerDataError(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
    }
}
