using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    private List<GameObject> pooledObjects;
    public GameObject[] objectToPool;
    public int amountToPool;

    void Awake()
    {
        // Loop through list of pooled objects,deactivating them and adding them to the list 
        pooledObjects = new List<GameObject>();
        for (int i = 0; i < amountToPool; i++)
        {
            GameObject obj = (GameObject)Instantiate(objectToPool[Random.Range(0, objectToPool.Length)]);
            obj.SetActive(false);
            pooledObjects.Add(obj);
            obj.transform.SetParent(this.transform); // set as children of Spawn Manager
        }
    }

    public GameObject GetPooledObject()
    {
        // For as many objects as are in the pooledObjects list
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            // if the pooled objects is NOT active, return that object 
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }
        // otherwise, return null   
        return null;
    }

    public GameObject GetRandomPooledObject()
    {
        if (!ObjectAvailable())
            return null;

        bool objectFound = false;
        int index = 0;
        // TODO: Improve performance by looping over fixed amount
        while (!objectFound)
        {
            index = Random.Range(0, pooledObjects.Count);
            if (!pooledObjects[index].activeInHierarchy)
            {
                objectFound = true;
            }
        }
        return objectFound ? pooledObjects[index] : null;
    }

    public bool ObjectAvailable()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return true;
            }
        }
        return false;
    }
}
