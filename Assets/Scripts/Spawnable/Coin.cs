using System;
using UnityEngine;
using UnityEngine.UI;

public class Coin : Spawnable
{
    public float rotationSpeed = 3f;
    public Text displayCoins;

    protected override void PerformPerFrameOperations()
    {
        transform.Translate(-SkyscraperSpawner.speed * Time.deltaTime, 0, 0, Space.World);

        // infinitely rotate this coin about the Y axis in world space
        transform.Rotate(0, rotationSpeed, 0, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        other.transform.parent.GetComponent<HeliController>().PickupCoin();
        // TODO: deactivate when object polling
        Destroy(gameObject);
    }


}
