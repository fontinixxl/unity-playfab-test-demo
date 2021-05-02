using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyScraperSpawner : Spawner
{
    [SerializeField]
    private float minY = -20f;
    [SerializeField]
    private float maxY = -15f;
    protected override IEnumerator Spawn()
    {
        while (true)
        {
            GameObject skyscraper = m_objectPooler.GetRandomPooledObject();

            skyscraper.transform.position = new Vector3(spawnX, Random.Range(minY, maxY), spawnZ);
            skyscraper.transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
            skyscraper.SetActive(true);

            // 25% chance increase the speed
            if (Random.Range(1, 4) == 1)
            {
                Speed += 1f;
            }

            // wait between 1-5 seconds for a new skyscraper to spawn
            yield return new WaitForSeconds(Random.Range(minSpawnTime, maxSpawnTime));
        }
    }
}
