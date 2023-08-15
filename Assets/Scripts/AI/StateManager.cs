using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.UIElements;
using UnityEngine;

public class StateManager : MonoBehaviour, IAmmunitionData
{
    // Current state to be processed.
    [SerializeField]
    private State _currentState;
    [SerializeField]
    private Transform _mainTarget;
    [SerializeField]
    private Transform _idlePosition;
    [SerializeField]
    private Pathfinder _pathfinder;
    [SerializeField]
    private Targetting _targetting;
    [SerializeField]
    private string _targetable;

    private MoveToTargetState _moveToTargetState;
    private ShipAttackShipState _shipAttackShipState;
    private ShipAttackBaseState _shipAttackBaseState;
    private SphereCollider _sphereCollider;
    // Ammo stuff
    private List<AmmunitionSO> _ammunitionList;


    public void Setup(AIInputManager inputManager, MovementSO movementData)
    {
        // Initialise all the states. We don't need a reference to them after the setup.
        _moveToTargetState = GetComponent<MoveToTargetState>();
        _shipAttackShipState = GetComponent<ShipAttackShipState>();
        _shipAttackBaseState = GetComponent<ShipAttackBaseState>();
        _sphereCollider = GetComponent<SphereCollider>();
        if (_moveToTargetState != null){
            _moveToTargetState.Setup(_mainTarget, _idlePosition, inputManager, _sphereCollider, _pathfinder, movementData, _targetable, _ammunitionList);
        }
        if (_shipAttackShipState != null){
            _shipAttackShipState.Setup(inputManager);
        }
        if (_shipAttackBaseState != null){
            _shipAttackBaseState.Setup(_mainTarget, inputManager, _targetting, _ammunitionList, _targetable);
        }
        if (_pathfinder != null){
            _pathfinder.Setup();
        }
    }

    // Is setup by cannonmanager.
    public void AmmunitionData(Dictionary<AmmunitionTypeEnum, AmmunitionSO> ammoDict)
    {
        _ammunitionList = ammoDict.Values.ToList();
        if (_ammunitionList.Count != Enum.GetValues(typeof(AmmunitionTypeEnum)).Length){
            Debug.LogError("ammoDictionary in StateManager does not equal the number of AmmoEnumTypes!");
        }
    }

    private void Update()
    {
        RunStateMachine();
    }
    // Get the next state from the current state.
    // When the current state is done call the next state.
    private void RunStateMachine(){
        State nextState = _currentState?.RunCurrentState();
        if (nextState != null && nextState != _currentState){
            SwitchToNextState(nextState);
        }
    }
    // Switch current state to next state.
    private void SwitchToNextState(State nextState){
        _currentState = nextState;
    }
}
