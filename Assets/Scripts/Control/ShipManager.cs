using PirateGame.Control;
using PirateGame.Health;
using PirateGame.Movement;
using UnityEngine;
using System.Collections.Generic;

public class ShipManager : MonoBehaviour
{
    [SerializeField]
    private ShipSO _shipData;
    [SerializeField]
    private MovementManager _movementManager;
    [SerializeField]
    private HealthManager _healthManager;
    [SerializeField]
    private SailHealth _sailHealth;
    [SerializeField]
    private HullHealth _hullHealth;
    [SerializeField]
    private CrewHealth _crewHealth;
    [SerializeField]
    private DamageHandler _damageManager;
    [SerializeField]
    private PlayerInputManager _inputManager;
    [SerializeField]
    private CannonManager _cannonManager;

    public ShipSO setShipData(ShipSO data) => _shipData = data;

    private void Start(){
        // Setup everything relating to the ship.
        if (_shipData == null) return;
        Setup();
    }
    private void Setup(){
        _healthManager.Setup((_sailHealth,_shipData.GetMaxSailHealth),
            (_hullHealth,_shipData.GetMaxHullHealth),
                (_crewHealth,_shipData.GetMaxCrewHealth));
    }
}
