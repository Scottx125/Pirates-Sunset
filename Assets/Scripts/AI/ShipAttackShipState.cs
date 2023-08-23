using System;
using System.Collections;
using System.Collections.Generic;
using PirateGame.Health;
using PirateGame.Helpers;
using UnityEngine;

public class ShipAttackShipState : State , IStructuralDamageModifier, ICorporealDamageModifier, IMobilityDamageModifier
{
    [SerializeField]
    private State _followState;
    [Header("AI Behaviour Weights")]
    [SerializeField]
    [Range(0, 1)]
    private float _structuralTargetWeight;
    [SerializeField]
    [Range(0, 1)]
    private float _corporealTargetWeight;
    [SerializeField]
    [Range(0, 1)]
    private float _mobilityTargetWeight;
    [SerializeField]
    [Range(0, 1)]
    // Will be applied to the current weight.
    private float _currentAttackTypeContinuationWeight;
    [Header("AI Own Health Weights")]
    [SerializeField]
    [Range(0, 1)]
    private float _shipHealthWeight;

    // Setup stuff
    private Transform _currentTarget;
    private AIInputManager _inputManager;
    private Pathfinder _pathfinder;
    private MovementSO _movementSO;
    private string _targetable;
    private AmmunitionSO _structuralDamageAmmo;
    private AmmunitionSO _corporealDamageAmmo;
    private AmmunitionSO _mobilityDamageAmmo;
    private HealthComponent[] _targetHealthComponenets;

    // Idea of the weight system below is the AI will have a range it wants to attack at based on it's desired type of attack.
    // If the AI lowers the enemy health to a certain point it will want to change attack based on it's current weights.
    // If the AI gets damaged it will change a weight and decide to do attacks that have longer range.

    // Target health
    private float _targetStructuralHealth;
    private float _targetCorporealHealth;
    private float _targetMobilityHealth;
    

    // My health
    // Will add to targeting weights, lower this is, the more the AI will want to stay far away.
    private float _structuralHealth;

    // Total weights for each decision.
    private float _corporealAttackDesire;
    private float _mobilityAttackDesire;
    private float _structuralAttackDesire;
    

    // Setup close, medium and far ranges based on ammo range.

    public void Setup(AIInputManager inputManager, Pathfinder pathfinder, MovementSO movementData, string targetable, List<AmmunitionSO> ammoList)
    {
        _inputManager = inputManager;
        _pathfinder = pathfinder;
        _movementSO = movementData;
        _targetable = targetable;
        _structuralDamageAmmo = AIHelpers.GetTopDamageOfType(ammoList, DamageTypeEnum.Structural);
        _corporealDamageAmmo = AIHelpers.GetTopDamageOfType(ammoList, DamageTypeEnum.Corporal);
        _mobilityDamageAmmo = AIHelpers.GetTopDamageOfType(ammoList, DamageTypeEnum.Mobility);
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

    public void CorporealDamageModifier(float modifier, string nameOfSender)
    {
        _targetCorporealHealth = modifier;
    }

    public void MobilityDamageModifier(float modifier, string nameOfSender)
    {
        _targetMobilityHealth = modifier;
    }

    public void StructuralDamageModifier(float modifier, string nameOfSender)
    {
        if (nameOfSender == transform.root.name)
        {
            _structuralHealth = modifier;
        } else
        {
            _targetStructuralHealth = modifier;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Update the ship target if it's in range.
        if (other.tag == _targetable)
        {
            _currentTarget = other.transform;
            RegisterToTargetHealthComponenets();
        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == _targetable)
        {
            _currentTarget = null;
            UnRegisterToTargetHealthComponenets();
        }
    }

    private void RegisterToTargetHealthComponenets()
    {
        // Search the target for it's health componenets and pass itself into them.
        _targetHealthComponenets = _currentTarget.transform.root.GetComponent<HealthManager>().GetHealthComponents().ToArray();
        foreach(HealthComponent componenet in _targetHealthComponenets){
            componenet.AddReciever(this.name, this);
        }
    }
    private void UnRegisterToTargetHealthComponenets()
    {
        foreach (HealthComponent componenet in _targetHealthComponenets)
        {
            componenet.RemoveReciever(this.name);
        }
    }

}
