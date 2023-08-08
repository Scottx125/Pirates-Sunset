using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    [SerializeField]
    private State _nextState;

    private bool _enemyInRange;

    public void Setup()
    {
        
    }
    public override State RunCurrentState(State? previousState)
    {
        if (_enemyInRange){
            _enemyInRange = false;
            return _nextState;
        } else {
            return this;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy"){
            _enemyInRange = true;
        }
    }
}
