using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State, IAmmunitionData
{
    [SerializeField]
    private State _nextState;

    private Transform _currentTarget;
    private AIInputManager _inputManager;
    private Dictionary<AmmunitionTypeEnum, AmmunitionSO> _ammunitionDict;
    // Setup close, medium and far ranges based on ammo range.

    public void Setup(AIInputManager inputManager)
    {
        _inputManager = inputManager;
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

    public void AmmunitionData(Dictionary<AmmunitionTypeEnum, AmmunitionSO> ammoDict)
    {
        // Once the cannnon manager is set up. Send a copy of the ammo dict here so
        // the AI knows what ammo is loaded.
        // This whilst using more memory reduces coupling between the cannon manager and
        // AI.
        _ammunitionDict = ammoDict;
    }
}
