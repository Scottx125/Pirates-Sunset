using PirateGame.Control;
using PirateGame.Health;
using PirateGame.Moving;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    [SerializeField]
    private bool _isAI;
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
    [SerializeField]
    private AIInputManager _aiInputManager;
    [SerializeField]
    private StateManager _stateManager;


    private void Awake(){
        Setup();
    }
    private void Setup(){
        if (_movementManager != null) _movementManager.Setup();
        if (_cannonManager != null) _cannonManager.Setup();
        if (_healthManager != null){
            ICorporealDamageModifier[] corporealDamageModifiers = {_movementManager, _cannonManager};
            IStructuralDamageModifier[] structuralDamageModifiers = {_movementManager};
            IMobilityDamageModifier[] mobilityDamageModifiers = {_movementManager};
            _healthManager.Setup(corporealDamageModifiers, structuralDamageModifiers, mobilityDamageModifiers);
        }
        if (_damageHandler != null) _damageHandler.Setup(_healthManager);
        if (_inputManager != null && !_isAI) _inputManager.Setup(_movementManager, _cameraController, _cannonManager);
        if (_aiInputManager != null && _isAI) _aiInputManager.Setup(_movementManager, _cannonManager);
        if (_stateManager != null && _isAI) _stateManager.Setup(_cannonManager, _aiInputManager);
        
    }
}
