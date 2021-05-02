using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneSpawner : Spawner
{
    protected override IEnumerator Spawn()
    {
        while (true)
        {
            GameObject plane = m_objectPooler.GetPooledObject();

            plane.transform.position = new Vector3(26, Random.Range(4.5f, 8.5f), 11);
            plane.transform.rotation = Quaternion.Euler(-90f, -90f, 0f);
            plane.SetActive(true);
           
            // pause this coroutine for 3-10 seconds and then repeat loop
            yield return new WaitForSeconds(Random.Range(3, 10));
        }
    }
}
