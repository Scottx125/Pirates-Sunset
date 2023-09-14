using PirateGame.Health;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
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

    // UIstuff
    [SerializeField]
    TextMeshProUGUI _currentWaveText;
    [SerializeField]
    TextMeshProUGUI _nextWaveTimer;
    [SerializeField]
    GameObject _nextWaveTimerRect;
    [SerializeField]
    TextMeshProUGUI _shipsRemainingText;
    [SerializeField]
    GameObject _player;

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
    private List<HealthComponent> _playerHealth = new List<HealthComponent>();
    

    private void Awake()
    {
        _instance = this;
        _currentLevelInt = _levelToStartOn;
        if (_player == null) 
        {
            Debug.LogError("Player is equal to NULL!");
            return;
        }
        GameManager.GetInstance().SetMaxWaves(_levels.Count);
        _playerHealth = _player.GetComponent<HealthManager>().GetHealthComponents();
    }

    private void Update()
    {
        if (!_startSpawning) return;
        if (_newLevel && _currentLevelInt <= _levels.Count)
        {
            SetupLevel();
        } else
        {
            // End Game
            EndGame();
        }
        // Spawn
        BeginSpawning();
        // Next level
        NextLevel();

    }

    private void BeginSpawning()
    {
        if (_spawningShips == null && _shipsToSpawn.Count > 0)
        {
            _spawningShips = StartCoroutine(SpawnShips());
        }
    }

    private void NextLevel()
    {
        if (_shipsToSpawn.Count == 0)
        {
            ForceNextLevel();
            LevelComplete();
        }
    }

    private void EndGame()
    {
        if (_shipsRemaining == 0 && _shipsToSpawn.Count == 0 && _currentLevelInt == _levels.Count)
        {
            GameManager.GetInstance().GameOver();
        }
    }

    private void LevelComplete()
    {
        if (_shipsRemaining == 0)
        {
            Timers();
            NextLevelUIText();
            if (_nextLevelTimer <= 0)
            {
                _newLevel = true;
                _currentLevelInt++;
            }
        }
    }

    private void NextLevelUIText()
    {
        if (_nextWaveTimerRect == null) return;
        if (_nextWaveTimerRect.activeInHierarchy == false && _currentLevelInt < _levels.Count)
        {
            _nextWaveTimerRect.SetActive(true);
        } 
        _nextWaveTimer.text = String.Format("{0}", _nextLevelTimer.ToString("F0"));
    }

    private void ForceNextLevel()
    {
        if (_bForceNextLevelAfterAllSpawned)
        {
            Timers();
            NextLevelUIText();
            if (_nextLevelTimer <= 0)
            {
                _newLevel = true;
                _currentLevelInt++;
            }
        }
    }

    private IEnumerator SpawnShips()
    {
        while (_shipsToSpawn.Count > 0)
        {
            int chosenSpawn = Random.Range(0, _spawnPositions.Count);
            int chosenShipToSpawn = Random.Range(0, _shipsToSpawn.Count);
            Instantiate(_shipsToSpawn[chosenShipToSpawn].GetShip, _spawnPositions[chosenSpawn].position, _spawnPositions[chosenSpawn].transform.rotation);
            _shipsToSpawn.RemoveAt(chosenShipToSpawn);
            yield return new WaitForSeconds(_spawnDelayTime);
        }
        _spawningShips = null;
    }

    private void Timers()
    {
        _nextLevelTimer -= Time.deltaTime;
    }

    private void SetupLevel()
    {
        // Set everything up for the new level.
        foreach (HealthComponent healthComp in _playerHealth)
        {
            healthComp.MaxHealth();
        }
        _nextWaveTimerRect.SetActive(false);
        _newLevel = false;
        _currentLevelData = _levels[_currentLevelInt - 1];
        if (_currentWaveText != null) _currentWaveText.text = String.Format("Wave: {0} of {1}", _currentLevelInt, _levels.Count);
        GameManager.GetInstance().SetWaveReached(_currentLevelInt);
        foreach (ShipToSpawnStruct ship in _currentLevelData.GetShipsToSpawn)
        {
            _shipsRemaining += ship.GetAmount;
            _shipsToSpawn.AddRange(Enumerable.Repeat(ship, ship.GetAmount));
        }
        UpdateShipsRemainingText();
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

    public void ShipDestroyed()
    {
        _shipsRemaining -= 1;
        UpdateShipsRemainingText();
    }

    private void UpdateShipsRemainingText()
    {
        if (_shipsRemainingText == null) return;
        _shipsRemainingText.text = String.Format("Ships left: {0}", _shipsRemaining);
    }
}
