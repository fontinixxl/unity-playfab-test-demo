using UnityEngine;

public class Skyscraper : MonoBehaviour {

    private void Update()
    {
		transform.Translate(-Spawner.Speed * Time.deltaTime, 0, 0);
    }
}
