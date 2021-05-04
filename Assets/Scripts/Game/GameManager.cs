using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    // UI
    public GameObject MainMenu;
    public GameObject GameOverPanel;
    public GameObject StatusBar;
    public Text StatusText;
    public Text GameOverCoins;

    // Game Elements
    public GameObject helicopter;
    public GameObject SpawnPool;
    private int sessionCoins;
    private DisplayCoins displayCoins;
    private AudioSource audioSource;
    private bool gamePlaying = false;
    private float roundTimer = 0;

    // PlayFab
    private PlayFabAuthService _AuthService;
    public GetPlayerCombinedInfoRequestParams InfoRequestParams;
    private Dictionary<string, int> GameStats = new Dictionary<string, int>();

    // Events to subscribe
    public static event Action StartGameEvent;
    public static event Action GameOverEvent;

    protected override void Awake()
    {
        base.Awake();
        _AuthService = PlayFabAuthService.Instance;
        displayCoins = StatusBar.GetComponentInChildren<DisplayCoins>();
        audioSource = GetComponent<AudioSource>();
    }

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

    private void Update()
    {
        if (gamePlaying)
        {
            roundTimer += Time.deltaTime;
        }
    }

    private void OnLoginSuccess(LoginResult result)
    {
        StatusText.text = "Logged In as: " + result.PlayFabId.ToString();
        MainMenu.SetActive(true);
    }

    // Invoked when Start Game button pressed (Main Menu Panel)
    public void StartGame()
    {
        // Reset data from previous sessions
        roundTimer = 0;
        sessionCoins = 0;

        PlayfabManager.UpdateStatistic("games_played", 1);

        StartGameEvent?.Invoke();
        // TODO: Move to SoundManager
        audioSource.Play();

        Instantiate(helicopter, new Vector3(1, 2, Spawner.spawnZ), helicopter.transform.rotation);
        StatusBar.SetActive(true);

        gamePlaying = true;

    }

    // Invoked when helicopter destroyed
    private void GameOver()
    {
        gamePlaying = false;
        // TODO: Move to SoundManager
        audioSource.Stop();
        GameOverEvent?.Invoke();

        // Disable Status Bar coins display
        StatusBar.SetActive(false);

        // Save coins collected in PlayFab
        PlayfabManager.SavePlayerData(sessionCoins, true);

        // Show game over menu
        GameOverCoins.text = string.Format("x{0}",sessionCoins.ToString());
        GameOverPanel.SetActive(true);

        // Update the final game states and Write all game states
        SetStat("total_coins_session", sessionCoins);
        SetStat("total_session_time", (int)roundTimer);
        WriteGameplayStats();
    }

    public void PickUpCoins()
    {
        sessionCoins++;
        displayCoins.RenderCoins(sessionCoins);
    }

    #region Player Stats Helper Methods
    public void ClearStats()
    {
        GameStats.Clear();
    }

    public void TallyStat(string stat)
    {
        AddToStat(stat, 1);
    }

    public void AddToStat(string stat, int value)
    {
        if (!GameStats.ContainsKey(stat))
        {
            GameStats[stat] = 0;
        }

        GameStats[stat] += value;
    }

    public int GetStat(string stat)
    {
        if (GameStats.ContainsKey(stat))
        {
            return GameStats[stat];
        }

        return -1;
    }

    public void SetStat(string stat, int value)
    {
        GameStats[stat] = value;
    }

    private void WriteGameplayStats()
    {
        var updateStats = new Dictionary<string, object>();

        foreach (var stat in GameStats.Keys)
        {
            updateStats[stat] = GameStats[stat];
        }

        PlayfabManager.UpdateStatistics(updateStats);
    }
    #endregion

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
