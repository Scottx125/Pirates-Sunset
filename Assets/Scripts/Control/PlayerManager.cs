using PirateGame.Control;
using PirateGame.Health;
using PirateGame.Moving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    private MovementManager _movementManager;
    [SerializeField]
    MovementSO _movementData;
    [SerializeField]
    private HealthManager _healthManager;
    [SerializeField]
    private DamageHandler _damageHandler;
    [SerializeField]
    private PlayerInputManager _inputManager;
    [SerializeField]
    private CannonManager _cannonManager;
    [SerializeField]
    private CameraController _cameraController;
    [SerializeField]
    private PlayerDeath _death;
    [SerializeField]
    private PlayerUIController _playerUIController;


    private void Awake()
    {
        Setup();
    }
    private void Setup()
    {
        if (_movementManager != null) _movementManager.Setup(_movementData, _playerUIController);
        if (_cannonManager != null) _cannonManager.Setup(null, _playerUIController, _playerUIController, _playerUIController);
        if (_inputManager != null) _inputManager.Setup(_movementManager, _cameraController, _cannonManager);
        if (_death != null) _death.Setup(_inputManager, _movementManager);
        if (_healthManager != null)
        {
                ICorporealDamageModifier[] corporealDamageModifiers = { _movementManager, _cannonManager, _playerUIController };
                IStructuralDamageModifier[] structuralDamageModifiers = { _movementManager, _death, _playerUIController };
                IMobilityDamageModifier[] mobilityDamageModifiers = { _movementManager, _playerUIController };
                _healthManager.Setup(corporealDamageModifiers, structuralDamageModifiers, mobilityDamageModifiers);
        }
        if (_damageHandler != null) _damageHandler.Setup(_healthManager);
    }
}
