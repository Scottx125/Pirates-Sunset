using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    // Current state to be processed.
    [SerializeField]
    private State _currentState;
    [SerializeField]
    private Transform _mainTarget;
    [SerializeField]
    private AIInputManager _inputManager;

    private State _previousState;
    private MoveToTargetState _moveToTargetState;

    private void Start()
    {
        // Initialise all the states. We don't need a reference to them after the setup.
        _moveToTargetState = GetComponent<MoveToTargetState>();
        _moveToTargetState.Setup(_mainTarget, _inputManager);
    }

    private void Update()
    {
        RunStateMachine();
    }
    // Get the next state from the current state.
    // When the current state is done call the next state.
    private void RunStateMachine(){
        State nextState = _currentState?.RunCurrentState(_previousState);
        if (nextState != null && nextState != _currentState){
            SwitchToNextState(nextState);
        }
    }
    // Switch current state to next state.
    private void SwitchToNextState(State nextState){
        _previousState = _currentState;
        _currentState = nextState;
    }
}
