using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Levels", menuName = "ScriptableObjects/Levels", order = 3)]
public class LevelSO : ScriptableObject
{
    [SerializeField]
    private bool _bSpawnDelay;
    [SerializeField]
    private bool _bForceNextLevelAfterAllSpawned;
    [SerializeField]
    private float _delayToNextLevelTime;
    [SerializeField]
    private float _spawnDelayTime;
    [SerializeField]
    private ShipToSpawnStruct[] _shipsToSpawn;

    public bool GetSpawnDelay => _bSpawnDelay;
    public bool GetForceNextLevelAfterAllSpawned => _bForceNextLevelAfterAllSpawned;
    public float GetDelayToNextLevelTime => _delayToNextLevelTime;
    public float GetSpawnDelayTime => _spawnDelayTime;
    public ShipToSpawnStruct[] GetShipsToSpawn => _shipsToSpawn;
}
