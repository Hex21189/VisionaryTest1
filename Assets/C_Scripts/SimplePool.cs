using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Creates a simple pool for handling a single object type. 
/// Note: does not work with multiple different spawnable objects.
/// </summary>
public class SimplePool : MonoBehaviour
{
    public GameObject poolablePrefab;
    private List<GameObject> available = new List<GameObject>();
    private List<GameObject> inUse = new List<GameObject>();

    /// <summary>
    /// Returns a unused pooled object or a new object if there are none available.
    /// </summary>
    /// <returns>New or reused object.</returns>
    public GameObject GetAvailableObject()
    {
        // lock if multithreading
        GameObject result = null;

        while (result == null && available.Count > 0)
        {
            result = available[0];
            available.RemoveAt(0);
        }
        
        if (result == null)
        {
            result = Instantiate(poolablePrefab);
        }

        inUse.Add(result);
        result.SetActive(true);

        return result;
    }

    /// <summary>
    /// Returns all objects that are currently in use.
    /// </summary>
    /// <returns>All objects in use.</returns>
    public List<GameObject> GetAllObjectsInUse()
    {
        return inUse;
    }

    /// <summary>
    /// Release an object back into the pool.
    /// </summary>
    /// <param name="pooledObject">Pooled object (if not already pooled it is added to the pool).</param>
    public void ReleaseObject(GameObject pooledObject)
    {
        pooledObject.SetActive(false);

        // lock if multithreading
        available.Add(pooledObject);
        inUse.Remove(pooledObject);
    }
}
