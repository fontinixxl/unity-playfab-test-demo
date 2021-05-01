using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public Coin coin;
    public Airplane airplane;
    public Skyscraper[] skyscrapers;

    public static float speed = 10f;


    private void OnEnable()
    {
        StartCoroutine(SpawnAirplanes());
        StartCoroutine(SpawnCoins());
        StartCoroutine(SpawnSkyscrapers());

    }

    private void OnDisable()
    {
        StopAllCoroutines();
        // Destroy already spawned gameobjects
        foreach (Transform child in transform) Destroy(child.gameObject);
    }

    IEnumerator SpawnCoins()
    {
        while (true)
        {
            // number of coins we could spawn vertically
            int coinsThisRow = Random.Range(1, 4);

            // instantiate all coins in this row separated by some random amount of space
            for (int i = 0; i < coinsThisRow; i++)
            {
                Instantiate(coin, new Vector3(32, Random.Range(-6, 8.5f), 11), Quaternion.identity, transform);
            }

            // pause 1-5 seconds until the next coin spawns
            yield return new WaitForSeconds(Random.Range(1, 5));
        }
    }
    
    IEnumerator SpawnSkyscrapers()
    {
        while (true)
        {

            Instantiate(skyscrapers[Random.Range(0, skyscrapers.Length)], new Vector3(26, Random.Range(-20, -15), 11),
                Quaternion.Euler(-90f, 0f, 0f), transform);

            // randomly increase the speed by 1
            if (Random.Range(1, 4) == 1)
            {
                speed += 1f;
            }

            // wait between 1-5 seconds for a new skyscraper to spawn
            yield return new WaitForSeconds(Random.Range(1, 5));
        }
    }

    IEnumerator SpawnAirplanes()
    {
        while (true)
        {

            // instantiate a random airplane past the right egde of the screen, facing left
            Instantiate(airplane, new Vector3(26, Random.Range(4.5f, 8.5f), 11),
                Quaternion.Euler(-90f, -90f, 0f), transform);

            // pause this coroutine for 3-10 seconds and then repeat loop
            yield return new WaitForSeconds(Random.Range(3, 10));
        }
    }
}
