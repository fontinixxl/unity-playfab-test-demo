using PlayFab.ClientModels;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : Singleton<MainMenuController>
{
    public Text Lives;
    public Text Coins;
    public Button StartButton;
    public Text CountDownTimerText;
    public Text CountDownTimerLaber;
    public Button BuyLivesButton;
    public Text UserName;
    public Text BundlePrice;

    private Coroutine timer;
    public bool HaveSetUserName = false;


    private void Start()
    {
        PlayfabManager.OnInventoryLoadEvent += OnInventoryLoad;
        BuyLivesButton.onClick.AddListener(OnClickBuyLives);
        BundlePrice.text = string.Format("*{0} Coins", PlayfabManager.livesBundlePrice);
    }

    private void Update()
    {
        if (!HaveSetUserName)
        {
            if (PlayfabManager.IsLoggedIn && PlayfabManager.IsAccountInfoLoaded)
            {
                if (!string.IsNullOrEmpty(PlayfabManager.UserDisplayName))
                {
                    UserName.text = PlayfabManager.UserDisplayName;
                }
                else
                {
                    UserName.text = "Visit Options to set your user name!";
                }

                HaveSetUserName = true;
            }
        }
    }

    void OnEnable()
    {
        PlayfabManager.GetInventory();
        ClearTextLabels();
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
        CountDownTimerLaber.text = string.Empty;
        CountDownTimerText.text = string.Empty;
        Lives.text = string.Empty;
        Coins.text = string.Empty;
    }

}
