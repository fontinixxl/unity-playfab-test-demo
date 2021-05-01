using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;
using PlayFab.PfEditor.Json;

public static class PlayfabManager 
{
    // Events to subscribe for this service

    // Flag set after successfull PlayFab Login
    public static bool IsLoggedIn = false;

    // The user's Title specific DisplayName
    public static string UserDisplayName = null;

    public static readonly Dictionary<string, int> virtualCurrency = new Dictionary<string, int>();

    public static void GetPlayerVirtualCurrency()
    {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), (GetUserInventoryResult result) =>
        {
            virtualCurrency.Clear();
            CacheVirtualCurrency(result.VirtualCurrency);

        }, (error) =>
        {
            Debug.LogError(error.ToString());
        });
    }

    public static void SavePlayerData(int coinsCollected)
    {
        var request = new ExecuteCloudScriptRequest
        {
            FunctionName = "SaveProgress",
            FunctionParameter = new { CoinsCollected = coinsCollected },
        };
        PlayFabClientAPI.ExecuteCloudScript(request, OnSavePlayerDataSuccess, OnSavePlayerDataError);
    }

    private static void OnSavePlayerDataSuccess(ExecuteCloudScriptResult result)
    {
        // Return JSON {"PlayFabId":"5AD1658EFE173B88","VirtualCurrency":"SC","BalanceChange":2,"Balance":14}
        //var JsonUtil = PluginManager.GetPlugin<ISerializerPlugin>(PluginContract.PlayFab_Serializer);
        //virtualCurrency.Clear();
        Debug.Log(JsonWrapper.SerializeObject(result.FunctionResult));
    }

    private static void OnSavePlayerDataError(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
    }

    public static void CacheVirtualCurrency(Dictionary<string,int> virtualCurrency)
    {
        foreach (var pair in virtualCurrency)
        {
            PlayfabManager.virtualCurrency.Add(pair.Key, pair.Value);
            Debug.LogFormat("key {0}, value {1}", pair.Key, pair.Value);
        }
    }

}
