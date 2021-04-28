using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;
using LoginResult = PlayFab.ClientModels.LoginResult;

public enum Authtypes
{
    None,
    Silent,
    //UsernameAndPassword,
}

public class PlayFabAuthService
{
    // Events to subscribe for this service
    public delegate void LoginSuccessEvent(LoginResult success);
    public static event LoginSuccessEvent OnLoginSuccess;
    
    public delegate void PlayFabErrorEvent(PlayFabError error);
    public static event PlayFabErrorEvent OnPlayFabError;

    // These are fields that we set when we are using the service.
    public string AuthTicket;
    public GetPlayerCombinedInfoRequestParams InfoRequestParams;

    // Accessbility for PlayFab ID & Session Tickets
    public static string PlayFabId { get { return _playFabId; } }
    private static string _playFabId;
    public static string SessionTicket { get { return _sessionTicket; } }
    private static string _sessionTicket;

    private const string _PlayFabAuthTypeKey = "PlayFabAuthType";

    public static PlayFabAuthService Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new PlayFabAuthService();
            }
            return _instance;
        }
    }
    private static PlayFabAuthService _instance;

    public PlayFabAuthService()
    {
        _instance = this;
    }

    /// <summary>
    /// Remember the type of authenticate for the user
    /// </summary>
    public Authtypes AuthType
    {
        get
        {
            return (Authtypes)PlayerPrefs.GetInt(_PlayFabAuthTypeKey, 0);
        }
        set
        {
            PlayerPrefs.SetInt(_PlayFabAuthTypeKey, (int)value);
        }
    }

    /// <summary>
    /// Authenticate the user by the Auth Type that was defined.
    /// </summary>
    public void Authenticate(Authtypes authType)
    {
        AuthType = authType;
        Authenticate();
    }

    /// <summary>
    /// Authenticate the user by the Auth Type that was defined.
    /// </summary>
    public void Authenticate()
    {
        Debug.LogFormat("Authenticating with {0}", AuthType.ToString());
        switch (AuthType)
        {
            case Authtypes.None:
                // If there is no Authtype we want to loggin silently anyway
                Authenticate(Authtypes.Silent);
                break;
            case Authtypes.Silent:
                SilentlyAuthenticate();
                break;
        }
    }

    private void SilentlyAuthenticate(System.Action<LoginResult> callback = null)
    {
        PlayFabClientAPI.LoginWithCustomID(new LoginWithCustomIDRequest()
        {
            TitleId = PlayFabSettings.TitleId,
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true,
            InfoRequestParameters = InfoRequestParams
        }, (result) => {
            //Store Identity and session
            _playFabId = result.PlayFabId;
            _sessionTicket = result.SessionTicket;

            //check if we want to get this callback directly or send to event subscribers.
            if (callback == null && OnLoginSuccess != null)
            {
                //report login result back to the subscriber
                OnLoginSuccess?.Invoke(result);
            }
            else if (callback != null)
            {
                //report login result back to the caller
                callback?.Invoke(result);
            }
        }, (error) => {
            //report errro back to the subscriber
            if (callback == null && OnPlayFabError != null)
            {
                OnPlayFabError.Invoke(error);
            }
            else
            {
                //make sure the loop completes, callback with null
                callback?.Invoke(null);
                //Output what went wrong to the console.
                Debug.LogError(error.GenerateErrorReport());
            }

        });
    }
}
