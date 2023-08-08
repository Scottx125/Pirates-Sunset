using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    [SerializeField]
    private State _nextState;

    private Transform _currentTarget;
    private AIInputManager _inputManager;
    private IAmmunitionData _ammunitionData;
    // Setup close, medium and far ranges based on ammo range.

    public void Setup(AIInputManager inputManager, IAmmunitionData ammoData)
    {
        _inputManager = inputManager;
        _ammunitionData = ammoData;
    }

    public override State RunCurrentState()
    {
        throw new System.NotImplementedException();
    }

    // If we're close range shoot grape shot.
    // If we're medium range shoot chain.
    // if we're long range shoot round.

    private void OnTriggerEnter(Collider other)
    {
        // Update the ship target if it's in range.
        _currentTarget = other.transform;
    }
}
