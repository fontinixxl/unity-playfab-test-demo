using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject SpawnPool;
    public ScrollingBackground scrollingBackround;

    // Panels
    public GameObject MainMenu;
    public GameObject GameOverPanel;
    public DisplayCoins CoinsDisplay;

    public GameObject helicopter;

    public Text StatusText;
    public Text GameOverCoins;
    //public Text TotalCoins;

    const string currencyCode = "SC";
    private int sessionCoins;
    private int coins;


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
        HeliController.OnCollision += GameOver;

        // Set the data we want at login from what we chose in our meta data.
        _AuthService.InfoRequestParams = InfoRequestParams;
        // Start the authentication process.
        _AuthService.Authenticate();
    }

    // Fired when Start Game button pressed (Main Menu Panel)
    public void StartGame()
    {
        // Reset previous collected coins
        sessionCoins = 0;
        
        Instantiate(helicopter, new Vector3(1, 2, Spawner.spawnZ), helicopter.transform.rotation);

        SpawnPool.SetActive(true);
        CoinsDisplay.gameObject.SetActive(true); // TODO: replace by HUD Panel
        scrollingBackround.enabled = true;
    }

    public void PickUpCoins()
    {
        sessionCoins++;
        CoinsDisplay.RenderCoins(sessionCoins);
    }

    /// <summary>
    /// Login Successfully
    /// </summary>
    /// <param name="result"></param>
    private void OnLoginSuccess(LoginResult result)
    {
        StatusText.text = "Logged In as: " + result.PlayFabId.ToString();
        MainMenu.SetActive(true);

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

    // Called when helicopter destroyed
    private void GameOver()
    {
        // Stop spawning planes, coins ans skyscrapers
        SpawnPool.SetActive(false);
        scrollingBackround.enabled = false;

        // Disable HUD coins display
        CoinsDisplay.gameObject.SetActive(false);

        // save coins collected in PlayFab
        PlayfabManager.SavePlayerData(sessionCoins);

        // Show game over menu
        GameOverCoins.text = sessionCoins.ToString();
        GameOverPanel.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

}
