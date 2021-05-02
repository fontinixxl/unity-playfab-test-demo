using System.Collections;
using UnityEngine;

public abstract class SpawnManager : MonoBehaviour
{
    public static float m_speed = 10f;
    protected ObjectPooler m_objectPooler;

    private void Awake()
    {
        m_objectPooler = GetComponent<ObjectPooler>();
    }

    private void OnEnable()
    {
        StartCoroutine(Spawn());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        foreach (Transform child in transform) child.gameObject.SetActive(false);
    }

    protected abstract IEnumerator Spawn();
}
