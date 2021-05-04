using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class CoinSpawner : Spawner
{
    // Min distance between coins
    private static readonly float coinsDistance = 2f;
    private static readonly float minYpos = -6.5f;
    private static readonly float maxYpos = 8f;
    private static readonly int maxCoins = Mathf.RoundToInt((Mathf.Abs(minYpos) + Mathf.Abs(maxYpos)) / coinsDistance);

    private List<float> coinsPosY;
    public int maxCoinsRow = 4;

    protected override void Awake()
    {
        base.Awake();
        coinsPosY = new List<float>(maxCoins);
    }

    protected override IEnumerator Spawn()
    {
        while (true)
        {
            coinsPosY.Clear();
            GenerateCoinSlots();
            // number of coins we could spawn vertically
            int coinsThisRow = Mathf.Clamp(Random.Range(1, maxCoinsRow), 1, maxCoins);

            // instantiate all coins in this row separated by some random amount of space
            for (int i = 0; i < coinsThisRow; i++)
            {
                GameObject coin = m_objectPooler.GetPooledObject();
                if (coin == null)
                    yield return null;

                int index = Random.Range(0, coinsPosY.Count);
                float yPos = coinsPosY[index];
                coinsPosY.RemoveAt(index);

                coin.transform.position = new Vector3(spawnX, yPos, spawnZ);
                coin.SetActive(true);
            }

            // pause 1-5 seconds until the next coin spawns
            yield return new WaitForSeconds(Random.Range(minSpawnTime, maxSpawnTime));
        }
    }

    private void GenerateCoinSlots()
    {
        float position = minYpos;
        for (int i = 0; i < maxCoins; i++)
        {
            coinsPosY.Add(position += coinsDistance);
        }

    }
}
