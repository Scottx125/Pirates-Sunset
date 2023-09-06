using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private static SpawnManager _instance;
    void Awake()
    {
        _instance = this;
    }

    public static SpawnManager GetInstance()
    {
        if (_instance == null)
            Debug.LogError("SpawnManager is NULL.");

        return _instance;
    }

    // Create a List to hold the levels SO.
    // Get all the data for the current level.
    // Then start spawning if we are allowed.
    // When a child dies remove a ship from the spawned ship int.
}
