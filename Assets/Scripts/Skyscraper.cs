using UnityEngine;

public class Skyscraper : Spawnable {

    protected override void Scroll()
    {
		transform.Translate(-SkyscraperSpawner.speed * Time.deltaTime, 0, 0);
    }
}
