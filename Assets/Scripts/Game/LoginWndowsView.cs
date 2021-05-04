using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;

public class LoginWndowsView : MonoBehaviour
{
    // Debug Flag to simulate a reset
    public bool ClearPlayerPrefs;

    // Meta fields for objects in the UI
    public InputField Username;
    public InputField Password;
    public InputField ConfirmPassword;
    public Toggle RememberMe;

    public Button LoginButton;
    public Button PlayAsGuestButton;
    public Button RegisterButton;
    public Button CancelRegisterButton;

    // Meta references to panels we need to show / hide
    public GameObject RegisterPanel;
    public GameObject SigninPanel;
    public GameObject Panel;
    public Text StatusText;

    public GetPlayerCombinedInfoRequestParams InfoRequestParams;

    // Reference to our Authentication service
    private PlayFabAuthService _AuthService = PlayFabAuthService.Instance;

    public void Awake()
    {
        if (ClearPlayerPrefs)
        {
            _AuthService.UnlinkSilentAuth();
            _AuthService.ClearRememberMe();
            _AuthService.AuthType = Authtypes.None;
        }

        //Set our remember me button to our remembered state.
        RememberMe.isOn = _AuthService.RememberMe;

        //Subscribe to our Remember Me toggle
        RememberMe.onValueChanged.AddListener(
            (toggle) =>
            {
                _AuthService.RememberMe = toggle;
            });
    }

    // The logic here is relevent when we are coming from Game Over menu
    // and the user is still playing as a Guest. This is a mesure to enforce the player
    // to register using email as a recovery method.
    // TODO: Display a message encouraging the player to register
    private void OnEnable()
    {
        if (!PlayfabManager.IsLoggedIn) return;

        StatusText.text = string.Empty;
        PlayAsGuestButton.GetComponentInChildren<Text>().text = "Continue as a Guest";
    }

    public void Start()
    {
        // Hide all our panels until we know what UI to display
        Panel.SetActive(false);
        RegisterPanel.SetActive(false);
        SigninPanel.SetActive(true);

        // Subscribe to events that happen after we authenticate
        PlayFabAuthService.OnDisplayAuthentication += OnDisplayAuthentication;
        PlayFabAuthService.OnLoginSuccess += OnLoginSuccess;
        PlayFabAuthService.OnPlayFabError += OnPlayFaberror;

        // Bind to UI buttons to perform actions when user interacts with the UI.
        LoginButton.onClick.AddListener(OnLoginClicked);
        PlayAsGuestButton.onClick.AddListener(OnPlayAsGuestClicked);
        RegisterButton.onClick.AddListener(OnRegisterButtonClicked);
        CancelRegisterButton.onClick.AddListener(OnCancelRegisterButtonClicked);

        // Set the data we want at login from what we chose in our meta data.
        _AuthService.InfoRequestParams = InfoRequestParams;

        // Start the authentication process.
        _AuthService.Authenticate();
    }

    /// <summary>
    /// Login Successfully - Goes to next screen.
    /// </summary>
    /// <param name="result"></param>
    private void OnLoginSuccess(LoginResult result)
    {
        Debug.LogFormat("Logged In as: {0}", result.PlayFabId);
        Panel.SetActive(false);

        PlayfabManager.IsLoggedIn = true;
        //PlayfabManager.LoadUserData();
        PlayfabManager.LoadAccountData();
        //PlayfabManager.LoadTitleData();

        GameManager.Instance.DisplayMainMenu();
    }

    /// <summary>
    /// Error handling for when Login returns errors.
    /// </summary>
    /// <param name="error"></param>
    private void OnPlayFaberror(PlayFabError error)
    {
        //There are more cases which can be caught, below are some
        //of the basic ones.
        switch (error.Error)
        {
            case PlayFabErrorCode.InvalidEmailAddress:
            case PlayFabErrorCode.InvalidPassword:
            case PlayFabErrorCode.InvalidEmailOrPassword:
            case PlayFabErrorCode.InvalidParams:
                StatusText.text = "Invalid Email or Password";
                break;

            case PlayFabErrorCode.AccountNotFound:
                RegisterPanel.SetActive(true);
                SigninPanel.SetActive(false);
                return;
            default:
                StatusText.text = error.GenerateErrorReport();
                break;
        }

        //Also report to debug console
        Debug.Log(error.Error);
        Debug.LogError(error.GenerateErrorReport());
    }

    /// <summary>
    /// Choose to display the Auth UI or any other action.
    /// </summary>
    private void OnDisplayAuthentication()
    {
        //Here we have choses what to do when AuthType is None.
        Panel.SetActive(true);
    }

    /// <summary>
    /// Play As a guest, which means they are going to silently authenticate
    /// by device ID or Custom ID
    /// </summary>
    private void OnPlayAsGuestClicked()
    {
        // If we are already logged in as a guest and the user want to continue as it's,
        // bring them to the main menu; no necessary to authenticate.
        if (PlayfabManager.IsLoggedIn)
        {
            Panel.SetActive(false);
            GameManager.Instance.DisplayMainMenu();
            return;
        }

        StatusText.text = "Logging In As Guest ...";
        _AuthService.Authenticate(Authtypes.Silent);
    }

    /// <summary>
    /// Login Button means they've selected to submit a username (email) / password combo
    /// Note: in this flow if no account is found, it will ask them to register.
    /// </summary>
    private void OnLoginClicked()
    {
        StatusText.text = string.Format("Logging In As {0} ...", Username.text);

        _AuthService.Email = Username.text;
        _AuthService.Password = Password.text;
        _AuthService.Authenticate(Authtypes.EmailAndPassword);
    }

    /// <summary>
    /// No account was found, and they have selected to register a username (email) / password combo.
    /// </summary>
    private void OnRegisterButtonClicked()
    {
        if (Password.text != ConfirmPassword.text)
        {
            StatusText.text = "Passwords do not Match.";
            return;
        }

        StatusText.text = string.Format("Registering User {0} ...", Username.text);

        _AuthService.Email = Username.text;
        _AuthService.Password = Password.text;
        _AuthService.Authenticate(Authtypes.RegisterPlayFabAccount);
    }

    /// <summary>
    /// They have opted to cancel the Registration process.
    /// Possibly they typed the email address incorrectly.
    /// </summary>
    private void OnCancelRegisterButtonClicked()
    {
        // Reset all forms
        Username.text = string.Empty;
        Password.text = string.Empty;
        ConfirmPassword.text = string.Empty;

        // Show panels
        RegisterPanel.SetActive(false);
        SigninPanel.SetActive(true);
    }
}
