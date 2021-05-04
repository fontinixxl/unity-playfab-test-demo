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
    public Text CountDownTimerText;
    public Text CountDownTimerLaber;
    public Button BuyLivesButton;

    private Coroutine timer;

    private void Start()
    {
        PlayfabManager.OnInventoryLoadEvent += OnInventoryLoad;
        BuyLivesButton.onClick.AddListener(OnClickBuyLives);
    }
    void OnEnable()
    {
        ClearTextLabels();
        PlayfabManager.GetInventory();
        StartButton.interactable = false;
        BuyLivesButton.interactable = false;
    }

    private void OnInventoryLoad(bool showCountdown)
    {
        ClearTextLabels();

        Lives.text = string.Format("x{0} ", PlayfabManager.LivesBalance);
        Coins.text = string.Format("x{0} ", PlayfabManager.CoinsBalance);

        StartButton.interactable = PlayfabManager.LivesBalance > 0;
        BuyLivesButton.interactable = PlayfabManager.CoinsBalance >= PlayfabManager.livesBundlePrice;

        if (showCountdown)
        {
            timer = StartCoroutine(CountDown());
        }
        else
        {
            StopCountdownIfOn();
            CountDownTimerLaber.text = "Max lives!";
        }
    }

    private IEnumerator CountDown()
    {
        CountDownTimerLaber.text = "Next lives in";
        DateTime nextFreeTicket = DateTime.Now.AddSeconds(PlayfabManager.SecondsToRecharge);

        while (nextFreeTicket.Subtract(DateTime.Now).TotalSeconds > 0)
        {
            CountDownTimerText.text = string.Format(" {0:n0} sec", nextFreeTicket.Subtract(DateTime.Now).TotalSeconds);
            yield return null;
        }

        CountDownTimerText.text = string.Empty;
        CountDownTimerLaber.text = "Fetching lives..";
        yield return new WaitForSeconds(1);

        PlayfabManager.GetInventory();
    }

    private void OnClickBuyLives()
    {
        BuyLivesButton.interactable = false;
        PlayfabManager.TryBuyLives();
    }

    private void OnDisable()
    {
        StopCountdownIfOn();
    }

    private void StopCountdownIfOn()
    {
        if (timer != null)
        {
            StopCoroutine(timer);
            CountDownTimerText.text = string.Empty;
        }
    }

    private void ClearTextLabels()
    {
        string loading = "Loading..";
        CountDownTimerLaber.text = loading;
        CountDownTimerText.text = string.Empty;
        Lives.text = loading;
        Coins.text = loading;
    }

}
