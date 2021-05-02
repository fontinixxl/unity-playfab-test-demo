using UnityEngine;

public class Airplane : MonoBehaviour 
{

    private void Start()
    {
        HeliController.OnCollision += OnCollideHelicopter;
    }

    void Update()
    {
		// twice as fast as the skyscraper spawner speed toward the left
		transform.Translate(-Spawner.Speed * 2 * Time.deltaTime, 0, 0, Space.World);
	}

    private void OnCollideHelicopter()
    {
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        HeliController.OnCollision -= OnCollideHelicopter;    
    }
}
