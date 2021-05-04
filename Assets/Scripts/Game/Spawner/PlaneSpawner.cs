using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneSpawner : Spawner
{
    [SerializeField]
    private float minY = 4.5f;
    [SerializeField]
    private float maxY = 8.5f;

    protected override IEnumerator Spawn()
    {
        while (true)
        {
            GameObject plane = m_objectPooler.GetPooledObject();

            plane.transform.position = new Vector3(spawnX, Random.Range(minY, maxY), spawnZ);
            plane.transform.rotation = Quaternion.Euler(-90f, -90f, 0f);
            plane.SetActive(true);
           
            // pause this coroutine for 3-10 seconds and then repeat loop
            yield return new WaitForSeconds(Random.Range(minSpawnTime, maxSpawnTime));
        }
    }
}
