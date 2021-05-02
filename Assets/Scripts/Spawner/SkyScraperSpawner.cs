using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyScraperSpawner : SpawnManager
{
    protected override IEnumerator Spawn()
    {
        while (true)
        {
            GameObject skyscraper = m_objectPooler.GetRandomPooledObject();

            skyscraper.transform.position = new Vector3(26, Random.Range(-20, -15), 11);
            skyscraper.transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
            skyscraper.SetActive(true);

            // 25% chance increase the speed
            if (Random.Range(1, 4) == 1)
            {
                m_speed += 1f;
            }

            // wait between 1-5 seconds for a new skyscraper to spawn
            yield return new WaitForSeconds(Random.Range(1, 5));
        }
    }
}
