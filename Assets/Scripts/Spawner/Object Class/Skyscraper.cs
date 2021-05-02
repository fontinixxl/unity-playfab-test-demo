using UnityEngine;

public class Skyscraper : Spawnable {

    protected override void PerformPerFrameOperations()
    {
		transform.Translate(-SpawnManager.m_speed * Time.deltaTime, 0, 0);
    }
}
