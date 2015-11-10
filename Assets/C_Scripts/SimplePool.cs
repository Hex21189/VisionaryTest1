using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SimplePool : MonoBehaviour
{
    public GameObject poolablePrefab;
    private List<GameObject> available = new List<GameObject>();
    private List<GameObject> inUse = new List<GameObject>();

    public GameObject GetAvailableObject()
    {
        // lock if multithreading
        GameObject result;

        if (available.Count > 0)
        {
            result = available[0];
            available.RemoveAt(0);
        }
        else
        {
            result = GameObject.Instantiate(poolablePrefab);
        }

        inUse.Add(result);
        result.SetActive(true);

        return result;
    }

    public void ReleaseObject(GameObject pooledObject)
    {
        pooledObject.SetActive(false);

        // lock if multithreading
        available.Add(pooledObject);
        inUse.Remove(pooledObject);
    }
}
