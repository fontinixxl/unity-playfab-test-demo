using UnityEngine;

public class Airplane : Spawnable {

    protected override void PerformPerFrameOperations()
    {
		// twice as fast as the skyscraper spawner speed toward the left
		transform.Translate(-SkyscraperSpawner.speed * 2 * Time.deltaTime, 0, 0, Space.World);
	}
}
