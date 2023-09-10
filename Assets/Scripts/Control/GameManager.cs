using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private float _timeBeforeStart = 30f;

    private int _score = 0;
    private int _shipsDestroyed = 0;
    private int _waveReached = 0;
    private static GameManager _instance;

    public void AddScore(int x) => _score += x;
    public void AddShipDestroyed(int x) => _shipsDestroyed += x;
    public void SetWaveReached(int x) => _waveReached += x;
    private void Awake()
    {
        _instance = this;
    }

    private void Update()
    {
        Timers();
        StartGame();
    }

    private void StartGame()
    {
        if (_timeBeforeStart <= 0)
        {
            SpawnManager.GetInstance().StartSpawning();
        }
    }

    private void Timers()
    {
        _timeBeforeStart -= Time.deltaTime;
    }

    public static GameManager GetInstance()
    {
        if (_instance == null)
            Debug.LogError("GameManager is NULL.");

        return _instance;
    }

    public void GameOver()
    {
        // Initiate score screen.
        // pause game etc.
    }

}
