using System.Collections;
using UnityEngine;

public abstract class Spawner : MonoBehaviour
{
    // X-coord position outside the screen to spawn
    public static readonly int spawnX = 26;
    public static readonly int spawnZ = 11;
    public static float Speed = 10f;
    protected ObjectPooler m_objectPooler;

    [SerializeField]
    protected float minSpawnTime = 1;
    [SerializeField]
    protected float maxSpawnTime = 5;

    protected void Awake()
    {
        m_objectPooler = GetComponent<ObjectPooler>();
    }

    private void OnEnable()
    {
        Speed = 10f;
        StartCoroutine(Spawn());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        foreach (Transform child in transform) child.gameObject.SetActive(false);
    }

    protected abstract IEnumerator Spawn();
}
