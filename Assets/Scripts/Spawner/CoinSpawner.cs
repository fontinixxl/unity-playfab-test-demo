using System.Collections;
using UnityEngine;

public class CoinSpawner : Spawner
{
    protected override IEnumerator Spawn()
    {
        //Debug.Log("Starting SpawnCoins Coroutine");
        while (true)
        {
            // number of coins we could spawn vertically
            int coinsThisRow = Random.Range(1, 4);

            // instantiate all coins in this row separated by some random amount of space
            for (int i = 0; i < coinsThisRow; i++)
            {
                GameObject coin = m_objectPooler.GetPooledObject();
                if (coin == null)
                    yield return null;

                coin.transform.position = new Vector3(32, Random.Range(-6, 8.5f), 11);
                coin.SetActive(true);
            }

            // pause 1-5 seconds until the next coin spawns
            yield return new WaitForSeconds(Random.Range(1, 5));
        }
    }
}
