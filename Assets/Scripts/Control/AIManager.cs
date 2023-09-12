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
    private EnemyDeath _death;
    [SerializeField]
    protected StateManager _stateManager;
    [SerializeField]
    private AIUIController _uiController;


    private void Awake(){
        Setup();
    }
    private void Setup(){
        if (_movementManager != null) _movementManager.Setup(_movementData);
        if (_cannonManager != null)
        {
            IAmmunitionData[] ammoData = { _stateManager, _uiController };
            _cannonManager.Setup(ammoData, null, null, null);
        }
        if (_aiInputManager != null) _aiInputManager.Setup(_movementManager, _cannonManager);
        if (_stateManager != null) _stateManager.Setup(_aiInputManager, _movementData);
        if (_death != null) _death.Setup(_stateManager.gameObject, _movementManager);
        if (_healthManager != null)
        {

            ICorporealDamageModifier[] corporealDamageModifiers = { _movementManager, _cannonManager, _uiController };
            IStructuralDamageModifier[] structuralDamageModifiers = { _movementManager, _stateManager.GetShipAttackStateForHealthSetup, _death, _uiController };
            IMobilityDamageModifier[] mobilityDamageModifiers = { _movementManager, _uiController };
            _healthManager.Setup(corporealDamageModifiers, structuralDamageModifiers, mobilityDamageModifiers);
        }
        if (_damageHandler != null) _damageHandler.Setup(_healthManager);
    }
}
