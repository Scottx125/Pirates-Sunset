using PirateGame.Control;
using PirateGame.Health;
using PirateGame.Moving;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    [SerializeField]
    private MovementManager _movementManager;
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

    private void Awake(){
        Setup();
    }
    private void Setup(){
        if (_movementManager != null) _movementManager.Setup();
        if (_healthManager != null) _healthManager.Setup(_movementManager);
        if (_damageHandler != null) _damageHandler.Setup(_healthManager);
        if (_inputManager != null) _inputManager.Setup(_movementManager, _cameraController, _cannonManager);
        if (_cannonManager != null) _cannonManager.Setup();
    }
}
