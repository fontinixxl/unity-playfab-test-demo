using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
	public GameObject[] prefabs;

	// Use this for initialization
	void Start()
	{
		PlayFabAuthService.OnLoginSuccess += OnLoginSuccess;
	}

	private void OnLoginSuccess(LoginResult success)
	{
		StartCoroutine(SpawnCoins());
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
				Instantiate(prefabs[Random.Range(0, prefabs.Length)], new Vector3(26, Random.Range(-10, 10), 11), Quaternion.identity);
			}

			// pause 1-5 seconds until the next coin spawns
			yield return new WaitForSeconds(Random.Range(1, 5));
		}
	}
}
