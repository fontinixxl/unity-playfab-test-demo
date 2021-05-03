using PlayFab.ClientModels;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public Text Lives;
    public Text Coins;
    public Button StartButton;
    public Text livesRegen;
    public Text livesRegenLabel;
    private Coroutine timer;

    void OnEnable()
    {
        PlayfabManager.OnInventoryLoadEvent += OnInventoryLoad;

        ClearTextLabels();
        PlayfabManager.GetInventory();
        StartButton.interactable = false;
    }

    private void OnInventoryLoad(bool showCountdown)
    {
        ClearTextLabels();
        Lives.text = "x " + PlayfabManager.LivesBalance;
        Coins.text = "x " + PlayfabManager.CoinsBalance;
        StartButton.interactable = PlayfabManager.LivesBalance > 0;

        if (showCountdown)
        {
            timer = StartCoroutine(CountDown());
        }
        else
        {
            livesRegenLabel.text = "Max lives!";
        }
    }

    private IEnumerator CountDown()
    {
        livesRegenLabel.text = "Next lives in";
        DateTime nextFreeTicket = DateTime.Now.AddSeconds(PlayfabManager.SecondsToRecharge);

        while (nextFreeTicket.Subtract(DateTime.Now).TotalSeconds > 0)
        {
            livesRegen.text = string.Format(" {0:n0} sec", nextFreeTicket.Subtract(DateTime.Now).TotalSeconds);
            yield return null;
        }

        livesRegen.text = string.Empty;
        livesRegenLabel.text = "Fetching lives..";
        yield return new WaitForSeconds(1);

        PlayfabManager.GetInventory();
    }

    private void OnDisable()
    {
        if (timer != null)
            StopCoroutine(timer);
    }

    private void ClearTextLabels()
    {
        livesRegenLabel.text = string.Empty;
        livesRegen.text = string.Empty;
        Lives.text = string.Empty;
        Coins.text = string.Empty;
    }

}
