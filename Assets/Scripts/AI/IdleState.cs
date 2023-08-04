using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    [SerializeField]
    private State _nextState;
    [SerializeField]
    private Transform _mainTarget;

    public void Setup()
    {
        
    }
    public override State RunCurrentState(State? previousState)
    {
        if (_mainTarget != null)
        {
            return _nextState;
        }
        else
        {
            return this;
        }
    }
}
