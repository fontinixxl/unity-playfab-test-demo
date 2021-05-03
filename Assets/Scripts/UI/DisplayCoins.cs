using UnityEngine;
using UnityEngine.UI;

public class DisplayCoins : MonoBehaviour
{
    private Text coinsDisplay;

    private void Awake()
    {
        coinsDisplay = GetComponent<Text>();
    }

    private void OnEnable()
    {
        coinsDisplay.text = "0";
    }

    public void RenderCoins(int coins)
    {
        coinsDisplay.text = coins.ToString();
    }
}
