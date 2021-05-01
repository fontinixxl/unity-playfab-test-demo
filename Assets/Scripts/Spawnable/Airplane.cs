using UnityEngine;

public class Airplane : Spawnable {

    private void Start()
    {
        HeliController.OnCollision += OnCollideHelicopter;
    }

    protected override void PerformPerFrameOperations()
    {
		// twice as fast as the skyscraper spawner speed toward the left
		transform.Translate(-SpawnManager.speed * 2 * Time.deltaTime, 0, 0, Space.World);
	}
    private void OnCollideHelicopter()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        HeliController.OnCollision -= OnCollideHelicopter;    
    }
}
