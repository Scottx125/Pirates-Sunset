using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private bool _startSpawning = false;
    [SerializeField]
    private bool _bForceNextLevelAfterAllSpawned;
    [SerializeField]
    private int _levelToStartOn = 1;
    [SerializeField]
    private List<LevelSO> _levels;
    [SerializeField]
    private List<Transform> _spawnPositions;

    private static SpawnManager _instance;

    private bool _newLevel = true;
    private int _currentLevelInt = 0;
    private Coroutine _spawningShips;
    private int _shipsRemaining;
    private float _spawnDelayTime;
    private float _nextLevelDelayTime;
    private float _nextLevelTimer;
    private LevelSO _currentLevelData;
    private List<ShipToSpawnStruct> _shipsToSpawn = new List<ShipToSpawnStruct>();
    

    private void Awake()
    {
        _instance = this;
        _currentLevelInt = _levelToStartOn;
    }

    private void Update()
    {
        if (!_startSpawning) return;
        if (_newLevel && _currentLevelInt <= _levels.Count)
        {
            Setup();
        }
        // Spawn
        if (_spawningShips == null && _shipsToSpawn.Count > 0)
        {
            _spawningShips = StartCoroutine(SpawnShips());
        }
        // Next level
        if (_shipsToSpawn.Count == 0)
        {
            if (_bForceNextLevelAfterAllSpawned)
            {
                Timers();
                if (_nextLevelTimer <= 0)
                {
                    _newLevel = true;
                    _currentLevelInt++;
                }

            }
            if (_shipsRemaining == 0)
            {
                Timers();
                if (_nextLevelTimer <= 0)
                {
                    _newLevel = true;
                    _currentLevelInt++;
                }

            }
        }
    }

    private IEnumerator SpawnShips()
    {
        while (_shipsToSpawn.Count > 0)
        {
            int chosenSpawn = Random.Range(0, _spawnPositions.Count);
            int chosenShipToSpawn = Random.Range(0, _shipsToSpawn.Count);
            Instantiate(_shipsToSpawn[chosenShipToSpawn].GetShip, _spawnPositions[chosenSpawn].position, Quaternion.identity);
            _shipsToSpawn.RemoveAt(chosenShipToSpawn);
            yield return new WaitForSeconds(_spawnDelayTime);
        }
        _spawningShips = null;
    }

    private void Timers()
    {
        _nextLevelTimer -= Time.deltaTime;
    }

    private void Setup()
    {
        // Set everything up for the new level.
        _newLevel = false;
        _currentLevelData = _levels[_currentLevelInt - 1];
        GameManager.GetInstance().SetWaveReached(_currentLevelInt);
        foreach (ShipToSpawnStruct ship in _currentLevelData.GetShipsToSpawn)
        {
            _shipsRemaining += ship.GetAmount;
            _shipsToSpawn.AddRange(Enumerable.Repeat(ship, ship.GetAmount));
        }
        _spawnDelayTime = _currentLevelData.GetSpawnDelayTime;
        _nextLevelDelayTime = _currentLevelData.GetSpawnDelayTime;
        _nextLevelTimer = _nextLevelDelayTime;
    }



    public static SpawnManager GetInstance()
    {
        if (_instance == null)
            Debug.LogError("SpawnManager is NULL.");

        return _instance;
    }

    public void StartSpawning()
    {
        _startSpawning = true;
    }

    // Create a List to hold the levels SO.
    // Get all the data for the current level.
    // Then start spawning if we are allowed.
    // When a child dies remove a ship from the spawned ship int.
}
