using UnityEngine;
using System.Collections;
using PlayFab.ClientModels;

public class SkyscraperSpawner : MonoBehaviour {

	public GameObject[] prefabs;
	public static float speed = 10f;

	void Start()
	{
		PlayFabAuthService.OnLoginSuccess += OnLoginSuccess;
	}

	private void OnLoginSuccess(LoginResult result)
	{
		StartCoroutine(SpawnSkyscrapers());
	}

	IEnumerator SpawnSkyscrapers()
	{

		while (true)
		{

			Instantiate(prefabs[Random.Range(0, prefabs.Length)], new Vector3(26, Random.Range(-20, -15), 11),
				Quaternion.Euler(-90f, 0f, 0f));

			// randomly increase the speed by 1
			if (Random.Range(1, 4) == 1)
			{
				speed += 1f;
			}

			// wait between 1-5 seconds for a new skyscraper to spawn
			yield return new WaitForSeconds(Random.Range(1, 5));
		}
	}
}
