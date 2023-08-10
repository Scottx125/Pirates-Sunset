using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipAttackShipState : State
{
    [SerializeField]
    private State _nextState;

    private Transform _currentTarget;
    private AIInputManager _inputManager;
    
    // Setup close, medium and far ranges based on ammo range.

    public void Setup(AIInputManager inputManager)
    {
        _inputManager = inputManager;
    }

    public override State RunCurrentState()
    {
        throw new System.NotImplementedException();
    }

    // Get the current damage modifiers from the target.
    // We will have a desired attack type value. This value will be based off of a custom value + the targets modifier offset.
    // So if we have a value of 1 on crew attack, .75 on sail and .5 on hull.
    // And all are healthy. The highest score will be what we choose to attack with, meaning we will get into close range and start shooting grape shot.
    // Each ship will have a desired range based on it's "Morale". This will be a default value + an average of it's combined health.
    // This will also determine if the ship wants to get up close and pepper the enemy or stay far away to avoid damage.
    // Bigger ships will be more confident and will stay closer, smaller ships will try to stay far away.


    // The ship will attempt to keep below 25% of it's max range.
    // The ship will aim to attack whatever it's range every x Seconds. 

    private void OnTriggerEnter(Collider other)
    {
        // Update the ship target if it's in range.
        _currentTarget = other.transform;
    }
}
