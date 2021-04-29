using UnityEngine;
using UnityEngine.UI;

public class DisplayCoins : MonoBehaviour
{
    private Text coinsDisplay;

    private void Start()
    {
        coinsDisplay = GetComponent<Text>();
        HeliController.OnCoinPickedUp += OnCoinPickedUp;
    }

    private void OnCoinPickedUp(int totalCoins)
    {
        coinsDisplay.text = "Session Coins: " + totalCoins;
    }
}
