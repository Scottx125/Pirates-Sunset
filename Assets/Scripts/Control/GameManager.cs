using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private float _timeBeforeStart = 30f;
    // Below refers to the wave timer.
    [SerializeField]
    TextMeshProUGUI _timeBeforeStartText;
    [SerializeField]
    GameObject _timeBeforeStartObject;

    private int _score = 0;
    private int _shipsDestroyed = 0;
    private int _waveReached = 0;
    private bool _gameStarted = false;
    private static GameManager _instance;

    public void AddScore(int x) => _score += x;
    public void AddShipDestroyed() => _shipsDestroyed += 1;
    public void SetWaveReached(int x) => _waveReached += x;
    private void Awake()
    {
        _instance = this;
    }

    private void Update()
    {
        Timers();
        StartCountdownText();
        StartGame();
    }

    private void StartCountdownText()
    {
        if (_timeBeforeStartText == null) return;
        if (_gameStarted) return;
        _timeBeforeStartText.text = String.Format("{0}", _timeBeforeStart.ToString("F0"));
    }

    private void StartGame()
    {
        if (_timeBeforeStart <= 0)
        {
            _gameStarted = true;
            if (_timeBeforeStartObject == null) return;
            _timeBeforeStartObject.SetActive(false);
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
