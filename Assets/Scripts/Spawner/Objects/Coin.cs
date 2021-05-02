using System;
using UnityEngine;
using UnityEngine.UI;

public class Coin : MonoBehaviour
{
    public float rotationSpeed = 3f;
    public Text displayCoins;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void Update()
    {
        transform.Translate(-Spawner.Speed * Time.deltaTime, 0, 0, Space.World);

        // infinitely rotate this coin about the Y axis in world space
        transform.Rotate(0, rotationSpeed, 0, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        gameManager.PickUpCoins();
        gameObject.SetActive(false);
    }


}
