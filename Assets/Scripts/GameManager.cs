using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Meta references to panels we need to show / hide
    public Text StatusText;

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

        // Set the data we want at login from what we chose in our meta data.
        _AuthService.InfoRequestParams = InfoRequestParams;
        // Start the authentication process.
        _AuthService.Authenticate();
    }

    /// <summary>
    /// Login Successfully - Goes to next screen.
    /// </summary>
    /// <param name="result"></param>
    private void OnLoginSuccess(PlayFab.ClientModels.LoginResult result)
    {
        Debug.LogFormat("Logged In as: {0}", result.PlayFabId);
        StatusText.text = "Logged In as: " + result.PlayFabId.ToString();
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

}
