using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject spawnManager;
    public ScrollingBackground scrollingBackround;

    // Panels
    public GameObject GameOverPanel;
    public GameObject CoinsDisplay;

    public GameObject helicopter;
    //public Text TotalCoins;

    const string currencyCode = "SC";
    private int sessionCoins;
    private int coins;

    public Text StatusText;
    //public Text HUDCoins;
    public Text GameOverCoins;

    private PlayFabAuthService _AuthService;
    
    // Settings for what data to get from playfab on login
    public GetPlayerCombinedInfoRequestParams InfoRequestParams;

    private void Awake()
    {
        _AuthService = PlayFabAuthService.Instance;
    }

    // Start is called before the first frame update
    void Start()
    {

        PlayFabAuthService.OnLoginSuccess += OnLoginSuccess;
        PlayFabAuthService.OnPlayFabError += OnPlayFaberror;
        HeliController.OnCollision += OnHelicopterCollision;

        sessionCoins = 0;

        // Set the data we want at login from what we chose in our meta data.
        _AuthService.InfoRequestParams = InfoRequestParams;
        // Start the authentication process.
        _AuthService.Authenticate();
    }

    public void PickUpCoins()
    {
        sessionCoins++;
        CoinsDisplay.GetComponent<Text>().text = "Coins: " + sessionCoins.ToString();
        //HUDCoins.text = sessionCoins.ToString();
    }

    /// <summary>
    /// Login Successfully
    /// </summary>
    /// <param name="result"></param>
    private void OnLoginSuccess(LoginResult result)
    {
        StatusText.text = "Logged In as: " + result.PlayFabId.ToString();

        scrollingBackround.enabled = true;
        spawnManager.SetActive(true);
        CoinsDisplay.SetActive(true);

        // Get coins from server (retrieved login)
        coins = PlayfabManager.virtualCurrency[currencyCode];
        //TotalCoins.text = "Total Coins = " + coins;

        Instantiate(helicopter);
        //LoginPanel.SetActive(false);
        //LoggedinPanel.SetActive(true);
        //UserName.text = result.InfoResultPayload.AccountInfo.Username ?? result.PlayFabId;
    }

    /// <summary>
    /// Error handling for when Login returns errors.
    /// </summary>
    /// <param name="error"></param>
    private void OnPlayFaberror(PlayFabError error)
    {
        //Basic error cases on Login
        switch (error.Error)
        {
            case PlayFabErrorCode.InvalidEmailAddress:
            case PlayFabErrorCode.InvalidPassword:
            case PlayFabErrorCode.InvalidEmailOrPassword:
                StatusText.text = "Invalid Email or Password";
                break;

            case PlayFabErrorCode.AccountNotFound:
                StatusText.text = "Account Not Found";
                //RegisterPanel.SetActive(true);
                //SigninPanel.SetActive(false);
                return;
            default:
                StatusText.text = error.GenerateErrorReport();
                break;
        }

        //Report to debug console
        Debug.Log(error.Error);
        Debug.LogError(error.GenerateErrorReport());
    }

    // TODO: Extrat to Game Over Method
    // Called when helicopter collisions with a collidable object
    private void OnHelicopterCollision()
    {
        // Stop spawning planes, coins ans skyscrapers
        spawnManager.SetActive(false);
        scrollingBackround.enabled = false;

        // Disable HUD coins display
        CoinsDisplay.SetActive(false);

        // save coins collected in PlayFab
        PlayfabManager.SavePlayerData(sessionCoins);

        // Show game over menu
        GameOverCoins.text = sessionCoins.ToString();
        GameOverPanel.SetActive(true);
    }

}
