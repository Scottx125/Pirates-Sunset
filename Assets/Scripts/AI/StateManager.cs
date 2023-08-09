using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public IAmmunitionData GetAmmunitionDataRequiree => _attackState;

    // Current state to be processed.
    [SerializeField]
    private State _currentState;
    [SerializeField]
    private Transform _mainTarget;
    [SerializeField]
    private Transform _idlePosition;
    [SerializeField]
    private AIInputManager _inputManager;
    [SerializeField]
    private Pathfinder _pathfinder;

    private MoveToTargetState _moveToTargetState;
    private AttackState _attackState;
    private SphereCollider _sphereCollider;

    public void Setup(AIInputManager inputManager)
    {
        // Initialise all the states. We don't need a reference to them after the setup.
        _moveToTargetState = GetComponent<MoveToTargetState>();
        _attackState = GetComponent<AttackState>();
        _sphereCollider = GetComponent<SphereCollider>();
        _inputManager = inputManager;
        if (_moveToTargetState != null){
            _moveToTargetState.Setup(_mainTarget, _idlePosition, _inputManager, _sphereCollider, _pathfinder);
        }
        if (_attackState != null){
            _attackState.Setup(_inputManager);
        }
        if (_pathfinder != null){
            _pathfinder.Setup();
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
