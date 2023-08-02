using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToTargetState : State
{
    // Current target (this is the base not the player)
    private Transform _mainTarget;
    private Transform _shipTarget;

    // Add firing state here.
    public void Setup()
    {
        // Setup the classes we need to interact with and also
        // the target.
    }
    public override State RunCurrentState(State? previousState)
    {
        // If main != null and ship == null && outside of range move closer.
        if (_mainTarget != null && _shipTarget == null)
        {
            // Move towards target
            return this;
        }
        if (_shipTarget != null)
        {
            // get within range, if the target is out of range, chase for X time and then set target to null.
            return this;
        }
        return this;
    }
}

// In this state we want to simply get the base location and move towards a certain range (cannon fire range).
// Then we will switch to the fire state.
