using PirateGame.Control;
using PirateGame.Health;
using PirateGame.Moving;
using UnityEngine;

public class AIManager: MonoBehaviour
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
    private CannonManager _cannonManager;
    [SerializeField]
    private AIInputManager _aiInputManager;
    [SerializeField]
    private StateManager _stateManager;
    [SerializeField]
    private EnemyDeath _death;


    private void Awake(){
        Setup();
    }
    private void Setup(){
        if (_movementManager != null) _movementManager.Setup(_movementData);
        if (_cannonManager != null) _cannonManager.Setup(_stateManager);
        if (_aiInputManager != null) _aiInputManager.Setup(_movementManager, _cannonManager);
        if (_stateManager != null) _stateManager.Setup(_aiInputManager, _movementData);
        if (_death != null) _death.Setup(_stateManager.gameObject, _movementManager);
        if (_healthManager != null)
        {

            ICorporealDamageModifier[] corporealDamageModifiers = { _movementManager, _cannonManager };
            IStructuralDamageModifier[] structuralDamageModifiers = { _movementManager, _stateManager.GetShipAttackStateForHealthSetup, _death};
            IMobilityDamageModifier[] mobilityDamageModifiers = { _movementManager };
            _healthManager.Setup(corporealDamageModifiers, structuralDamageModifiers, mobilityDamageModifiers);
        }
        if (_damageHandler != null) _damageHandler.Setup(_healthManager);
    }
}
