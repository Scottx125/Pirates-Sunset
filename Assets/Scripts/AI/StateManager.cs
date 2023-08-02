using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    // Current state to be processed.
    [SerializeField]
    private State _currentState;
    private State _previousState;

    public void Setup()
    {
        // Initialise all the states. We don't need a reference to them after the setup.
    }

    private void Update()
    {
        RunStateMachine();
    }
    // Get the next state from the current state.
    // When the current state is done call the next state.
    private void RunStateMachine(){
        State nextState = _currentState?.RunCurrentState(_previousState);
        if (nextState != null){
            SwitchToNextState(nextState);
        }
    }
    // Switch current state to next state.
    private void SwitchToNextState(State nextState){
        _currentState = nextState;
    }
}
