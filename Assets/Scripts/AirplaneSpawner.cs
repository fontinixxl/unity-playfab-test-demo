using UnityEngine;
using System.Collections;
using PlayFab.ClientModels;

public class AirplaneSpawner : MonoBehaviour {
	public GameObject[] prefabs;

	// Use this for initialization
	void Start () {

		PlayFabAuthService.OnLoginSuccess += OnLoginSuccess;
	}

    private void OnLoginSuccess(LoginResult success)
    {
		StartCoroutine(SpawnAirplanes());
    }

	IEnumerator SpawnAirplanes() {
		while (true) {

			// instantiate a random airplane past the right egde of the screen, facing left
			Instantiate(prefabs[Random.Range(0, prefabs.Length)], new Vector3(26, Random.Range(7, 10), 11),
				Quaternion.Euler(-90f, -90f, 0f));

			// pause this coroutine for 3-10 seconds and then repeat loop
			yield return new WaitForSeconds(Random.Range(3, 10));
		}
	}
}
