using PirateGame.Control;
using PirateGame.Health;
using PirateGame.Movement;
using UnityEngine;

public class ShipManager : MonoBehaviour
{
    [SerializeField]
    private ShipSO _shipData;
    
    private Mover _mover;
    private Health _health;
    private DamageManager _damageManager;
    private PlayerController _playerController;

    public ShipSO setShipData(ShipSO data) => _shipData = data;

    private void Start(){
        // Setup everything relating to the ship.
        if (_shipData == null) return;
        if (_health == null) _health = GetComponent<Health>();
        if (_mover == null) _mover = GetComponent<Mover>();
        if (_damageManager == null) _damageManager = GetComponent<DamageManager>();
        if (_playerController == null) _playerController = GetComponent<PlayerController>();

        Setup();
    }
    private void Setup(){
        _health.Setup(_shipData.GetHullHealth, _shipData.GetSailHealth, _shipData.GetCrewHealth);
        _mover.Setup(_shipData.GetMoverDataStruct);
        _damageManager.Setup();
        _playerController.Setup();
    }
}
