using PirateGame.Control;
using PirateGame.Health;
using PirateGame.Moving;
using UnityEngine;

public class ShipManager : MonoBehaviour
{
    [SerializeField]
    private MovementManager _movementManager;
    [SerializeField]
    private HealthManager _healthManager;
    [SerializeField]
    private DamageHandler _damageManager;
    [SerializeField]
    private PlayerInputManager _inputManager;
    [SerializeField]
    private CannonManager _cannonManager;

    private void Awake(){
        Setup();
    }
    private void Setup(){
        if (_movementManager != null) _movementManager.Setup();
        if (_healthManager != null) _healthManager.Setup(_movementManager);
        if (_damageManager != null) _damageManager.Setup(_healthManager);
        if (_inputManager != null) _inputManager.Setup(_movementManager);
        if (_cannonManager != null) _cannonManager.Setup();
    }
}
