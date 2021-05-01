using System;
using UnityEngine;
using UnityEngine.UI;

public class Coin : Spawnable
{
    public float rotationSpeed = 3f;
    public Text displayCoins;
    private GameManager Manager;

    private void Start()
    {
        Manager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    protected override void PerformPerFrameOperations()
    {
        transform.Translate(-SpawnManager.speed * Time.deltaTime, 0, 0, Space.World);

        // infinitely rotate this coin about the Y axis in world space
        transform.Rotate(0, rotationSpeed, 0, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        Manager.PickUpCoins();
        // TODO: deactivate when object polling
        Destroy(gameObject);
    }


}
