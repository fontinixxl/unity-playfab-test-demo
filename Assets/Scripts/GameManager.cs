using PlayFab;
using PlayFab.ClientModels;
using System;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // UI
    public GameObject MainMenu;
    public GameObject GameOverPanel;
    public GameObject StatusBar;
    public Text StatusText;
    public Text GameOverCoins;

    // Gameplay Elements
    public GameObject helicopter;
    public GameObject SpawnPool;
    private int sessionCoins;
    private DisplayCoins displayCoins;

    //const string currencyCode = "SC";
    //private int coins;
    
    // PlayFab
    private PlayFabAuthService _AuthService;
    public GetPlayerCombinedInfoRequestParams InfoRequestParams;
    
    // Events to subscribe
    public static event Action StartGameEvent;
    public static event Action GameOverEvent;

    private void Awake()
    {
        _AuthService = PlayFabAuthService.Instance;
        displayCoins = StatusBar.GetComponentInChildren<DisplayCoins>();
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

        StartGameEvent?.Invoke();

        Instantiate(helicopter, new Vector3(1, 2, Spawner.spawnZ), helicopter.transform.rotation);

        StatusBar.SetActive(true);
    }

    // Called when helicopter destroyed
    private void GameOver()
    {
        GameOverEvent?.Invoke();

        // Disable Status Bar coins display
        StatusBar.SetActive(false);

        // Save coins collected in PlayFab
        PlayfabManager.SavePlayerData(sessionCoins);

        // Show game over menu
        GameOverCoins.text = sessionCoins.ToString();
        GameOverPanel.SetActive(true);
    }

    public void PickUpCoins()
    {
        sessionCoins++;
        displayCoins.RenderCoins(sessionCoins);
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


    public void ExitGame()
    {
        Application.Quit();
    }

}
