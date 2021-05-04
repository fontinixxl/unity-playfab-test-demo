using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    private InputField CurrentInput;
    private string PreviousName;
    public Button BackButton;
    public Button ClearSigninButton;

    private void Start()
    {
        BackButton.onClick.AddListener(SaveOptions);
        ClearSigninButton.onClick.AddListener(ClearUserSignin);

        // Keep track of the User Name input
        CurrentInput = transform.Find("InputField").GetComponent<InputField>();

        // Set the UI with the current settings
        CurrentInput.text = PlayfabManager.UserDisplayName;
        PreviousName = CurrentInput.text;
    }

    public void SaveOptions()
    {
        // Write the current values to the service

        if (PreviousName != CurrentInput.text)
        {
            Debug.Log("Set Username : " + CurrentInput.text);
            PlayfabManager.SetUserDisplayName(CurrentInput.text);
            PreviousName = CurrentInput.text;
        }

        MainMenuController.Instance.HaveSetUserName = false;
    }

    private void ClearUserSignin()
    {
        PlayFabAuthService.Instance.ClearRememberMe();
    }
}
