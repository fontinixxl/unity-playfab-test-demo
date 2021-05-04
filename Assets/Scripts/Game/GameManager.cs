using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    // UI
    public GameObject MainMenuPanel;
    public GameObject LoginPanel;
    public GameObject GameOverPanel;
    public GameObject StatusBarPanel;
    public Text GameOverCoins;

    // Game Elements
    public GameObject helicopter;
    public GameObject SpawnPool;
    private int sessionCoins;
    private DisplayCoins displayCoins;
    private bool gamePlaying = false;
    private float roundTimer = 0;

    // PlayFab
    public bool HaveSetUserName = false;
    private Dictionary<string, int> GameStats = new Dictionary<string, int>();

    // Events to subscribe
    public static event Action StartGameEvent;
    public static event Action GameOverEvent;

    protected override void Awake()
    {
        base.Awake();
        displayCoins = StatusBarPanel.GetComponentInChildren<DisplayCoins>();
    }

    void Start()
    {
        HeliController.OnCollision += GameOver;
    }

    private void Update()
    {
        if (gamePlaying)
        {
            roundTimer += Time.deltaTime;
        }
    }

    public void DisplayMainMenu()
    {
        MainMenuPanel.SetActive(true);
    }

    // Invoked when Start Game button pressed (Main Menu Panel)
    public void StartGame()
    {
        // Reset data from previous sessions
        roundTimer = 0;
        sessionCoins = 0;

        PlayfabManager.UpdateStatistic("games_played", 1);

        StartGameEvent?.Invoke();

        Instantiate(helicopter, new Vector3(1, 2, Spawner.spawnZ), helicopter.transform.rotation);
        StatusBarPanel.SetActive(true);

        gamePlaying = true;

    }

    public void ReturningFromGameOverMenu()
    {
        // If the user is not yet logged in useding email/password, show login menu
        if (PlayFabAuthService.Instance.AuthType.Equals(Authtypes.Silent))
        {
            LoginPanel.SetActive(true);
        }
        else
        {
            DisplayMainMenu();
        }
    }

    // Invoked when helicopter destroyed
    private void GameOver()
    {
        gamePlaying = false;

        GameOverEvent?.Invoke();

        // Disable Status Bar coins display
        StatusBarPanel.SetActive(false);

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

    public void ExitGame()
    {
        Application.Quit();
    }
}
