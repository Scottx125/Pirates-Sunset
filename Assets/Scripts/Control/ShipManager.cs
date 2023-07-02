using PirateGame.Control;
using PirateGame.Health;
using PirateGame.Movement;
using UnityEngine;

[RequireComponent(typeof(MovementManager))]
[RequireComponent(typeof(HealthManager))]
[RequireComponent(typeof(DamageManager))]
[RequireComponent(typeof(InputManager))]
public class ShipManager : MonoBehaviour
{
    [SerializeField]
    private ShipSO _shipData;
    [SerializeField]
    private MovementManager _movementManager;
    [SerializeField]
    private HealthManager _healthManager;
    [SerializeField]
    private DamageManager _damageManager;
    [SerializeField]
    private InputManager _inputManager;

    public ShipSO setShipData(ShipSO data) => _shipData = data;

    private void Start(){
        // Setup everything relating to the ship.
        if (_shipData == null) return;
        Setup();
    }
    private void Setup(){
        _movementManager.Setup(_shipData.GetMoverDataStruct);
        _healthManager.Setup(_shipData.GetMaxCrewHealth,_shipData.GetMaxHullHealth,_shipData.GetMaxSailHealth);
        _damageManager.Setup(_healthManager);
        _inputManager.Setup();
    }
}
