using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public struct ShipToSpawnStruct
{
    [SerializeField]
    GameObject _shipToSpawn;
    [SerializeField]
    int _amount;

    public int GetAmount => _amount;
    public GameObject GetShip => _shipToSpawn;
}
