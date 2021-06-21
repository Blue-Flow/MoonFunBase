using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton : MonoBehaviour
{
    private void Awake()
    {
        // Set the singleton to avoid problems when reloading
        int singletonCount = FindObjectsOfType<Singleton>().Length;
        if (singletonCount > 1)
        { Destroy(gameObject); }
        else DontDestroyOnLoad(gameObject);
    }
}
