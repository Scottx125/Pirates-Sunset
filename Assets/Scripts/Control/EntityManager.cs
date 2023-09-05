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
    private AIInputManager _aiInputManager;
    [SerializeField]
    private StateManager _stateManager;
    [SerializeField]
    private DeathAbstract _death;


    private void Awake(){
        Setup();
    }
    private void Setup(){
        if (_movementManager != null) _movementManager.Setup(_movementData);
        if (_cannonManager != null && !_isAI) _cannonManager.Setup(null);
        if (_cannonManager != null && _isAI) _cannonManager.Setup(_stateManager);
        if (_aiInputManager != null && _isAI) _aiInputManager.Setup(_movementManager, _cannonManager);
        if (_stateManager != null && _isAI) _stateManager.Setup(_aiInputManager, _movementData);
        if (_inputManager != null && !_isAI) _inputManager.Setup(_movementManager, _cameraController, _cannonManager);
        if (_death != null)
        {
            switch (_death)
            {
                case EnemyDeath ai:
                    ai.Setup(_stateManager.gameObject, _movementManager);
                    break;
                case PlayerDeath player:
                    player.Setup(_inputManager, _movementManager);
                    break;
                default:
                    break;
            }
        }
        if (_healthManager != null)
        {
            if (_isAI)
            {
                ICorporealDamageModifier[] corporealDamageModifiers = { _movementManager, _cannonManager };
                IStructuralDamageModifier[] structuralDamageModifiers = { _movementManager, _stateManager.GetShipAttackStateForHealthSetup, _death};
                IMobilityDamageModifier[] mobilityDamageModifiers = { _movementManager };
                _healthManager.Setup(corporealDamageModifiers, structuralDamageModifiers, mobilityDamageModifiers);
            } else
            {
                ICorporealDamageModifier[] corporealDamageModifiers = { _movementManager, _cannonManager };
                IStructuralDamageModifier[] structuralDamageModifiers = { _movementManager, _death};
                IMobilityDamageModifier[] mobilityDamageModifiers = { _movementManager };
                _healthManager.Setup(corporealDamageModifiers, structuralDamageModifiers, mobilityDamageModifiers);
            }
            
            
        }
        if (_damageHandler != null) _damageHandler.Setup(_healthManager);
    }
}
