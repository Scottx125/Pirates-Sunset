using System;
using System.Collections;
using System.Collections.Generic;
using PirateGame.Health;
using PirateGame.Helpers;
using PirateGame.Moving;
using UnityEngine;

public class ShipAttackShipState : State , IStructuralDamageModifier, ICorporealDamageModifier, IMobilityDamageModifier
{
    [SerializeField]
    private float _shipRangeOffset = 30f;
    [SerializeField]
    private float _shipAttackWaypointOffset = 15f;
    [SerializeField]
    private float _maxAngleToReduceSpeed = 67.5f;
    [SerializeField]
    private float _timeBetweenAttackAttempts = 6f;
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
    private MovementSO _movementData;
    private string _targetable;
    private HealthComponent[] _targetHealthComponenets;
    private Targetting _targetting;
    private State _state;
    private Vector3? _currentWaypoint;
    private Vector3? _attackWaypointPos;
    private float _timeSinceLastAttackWhileInRange = 0f;
    AttackTypesStruct _structuralAttack = new AttackTypesStruct();
    AttackTypesStruct _mobiltiyAttack = new AttackTypesStruct();
    AttackTypesStruct _corporealAttack = new AttackTypesStruct();
    AttackTypesStruct _chosenAttack;

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
    

    public void Setup(AIInputManager inputManager, Pathfinder pathfinder, MovementSO movementData, string targetable, List<AmmunitionSO> ammoList, Targetting targetting)
    {
        _inputManager = inputManager;
        _pathfinder = pathfinder;
        _movementData = movementData;
        _targetable = targetable;
        _targetting = targetting;
        _structuralAttack.Setup(AIHelpers.GetTopDamageOfType(ammoList, DamageTypeEnum.Structural));
        _mobiltiyAttack.Setup(AIHelpers.GetTopDamageOfType(ammoList, DamageTypeEnum.Mobility));
        _corporealAttack.Setup(AIHelpers.GetTopDamageOfType(ammoList, DamageTypeEnum.Corporal));
    }

    public override State RunCurrentState()
    {
        
        _state = CheckRange();
        if (_state == _followState) return _state;
        CalculatedDesiredAttack();
        PathToDesiredAttackRange();
        AttackBehaviour();
        return this;
    }

    private void Update()
    {
        _timeSinceLastAttackWhileInRange += Time.deltaTime;
    }

    private void AttackBehaviour()
    {
        if (_currentWaypoint == null) return;
        // Calculate waypoint if it's not done.
        // If we're close to the range waypoint and haven't created a waypoint.
        // Or If we're within max range, time elapsed is greater than the delay and we've not created a waypoint.
        if (_timeSinceLastAttackWhileInRange >= _timeBetweenAttackAttempts && Vector3.Distance(_currentTarget.position, transform.position) <= _chosenAttack.GetAmmoData.GetMaxRange - _shipRangeOffset)
        {
            if (_attackWaypointPos == null)
            {
                // Calc waypoint..
                _attackWaypointPos = CalculateAttackWaypoint();
            }
            // Path to attack waypoint
            _currentWaypoint = _pathfinder.PathToTarget(transform, _attackWaypointPos);
            _inputManager.MovementInput(SpeedModifierEnum.Full_Sails);
            _inputManager.Rotation((Vector3)_currentWaypoint, transform.forward);
            // Calculate target firing position.
            Vector3 shootingOffsetPos = _targetting.Target(_currentTarget, _chosenAttack.GetAmmoData.GetSpeed);
            // Calc direciton to target
            Vector3 directionToTarget = shootingOffsetPos - transform.position;
            // Calc angle between forward vector and the direction.
            float angleToShoot = Vector3.Angle(transform.forward, directionToTarget);
            if (angleToShoot >= 87.5f || angleToShoot <= 92.5f)
            {
                _inputManager.Fire(CalcDirectionToShoot(), _chosenAttack.GetAmmoData.GetAmmunitionType);
            }
            // Reset behaviour and go to get into range.
            if (Vector3.Distance(transform.position, (Vector3)_attackWaypointPos) < 0.1f || _timeSinceLastAttackWhileInRange >= _timeBetweenAttackAttempts * 2)
            {
                _timeSinceLastAttackWhileInRange = 0f;
                _attackWaypointPos = null;
            }

        }
    }
    private Vector3 CalculateAttackWaypoint()
    {

        // THIS ISN'T WORKING, LOOK AT FIXING THIS NEXT.
        // Calc attack waypoint.
        // Get position we need to aim for.
        Vector3 shootingOffsetPos = _targetting.Target(_currentTarget, _chosenAttack.GetAmmoData.GetSpeed);
        // Calc direciton to target
        Vector3 directionToTarget = shootingOffsetPos - transform.position;
        // Get the angle from the front of the ship to the shooting position.
        float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);
        // Get direction to shoot.
        CannonPositionEnum directionToShoot = CalcDirectionToShoot();
        // Take the angle and turn it into a left or right waypoint.
        float angleInRadians = Mathf.Abs((90f - angleToTarget) * Mathf.Deg2Rad);
        if (directionToShoot == CannonPositionEnum.Left)
        {
            // Waypoint Right
            float xOffset = 1 * Mathf.Cos(angleInRadians) * _shipAttackWaypointOffset;
            float zOffset = 1 * Mathf.Cos(angleInRadians) * _shipAttackWaypointOffset;
            return (Vector3)(_attackWaypointPos = transform.position + new Vector3(xOffset, 0f, zOffset));

        }
        else
        {
            // Waypoint Left
            float xOffset = -1 * Mathf.Cos(angleInRadians) * _shipAttackWaypointOffset;
            float zOffset = -1 * Mathf.Cos(angleInRadians) * _shipAttackWaypointOffset;
            return (Vector3)(_attackWaypointPos = transform.position + new Vector3(xOffset, 0f, zOffset));
        }
    }

    private CannonPositionEnum CalcDirectionToShoot()
    {
        Vector3 crossProduct = Vector3.Cross(transform.forward, _currentTarget.position);
        if (crossProduct.y < 0)
        {
            return CannonPositionEnum.Right;
        }
        else
        {
            return CannonPositionEnum.Left;
        }
    }

    private void PathToDesiredAttackRange()
    {
        // This is where if we're in range of our waypoint (within 10f) we 
        // Calcualte the target position we need to get to taking into account the lead and then trigger the attack.
        // This will be as simple as ordering the ship to go to a new waypoint .
        // Then create a waypoint based on lining up our cannons to the enemy.
        if (_attackWaypointPos != null) return;
        // Get position we need to aim for.
        Vector3 shootingOffsetPos = _targetting.Target(_currentTarget, _chosenAttack.GetAmmoData.GetSpeed);
        // Calculate direction to that position.
        Vector3 directionToTarget = (shootingOffsetPos - transform.position).normalized;
        // Calculate the position we need to path to along that direciton.
        Vector3 wayPoint = directionToTarget * (_chosenAttack.GetAmmoData.GetMaxRange - _shipRangeOffset);
        // Calculate the path to the waypoint.
        _currentWaypoint = _pathfinder.PathToTarget(_currentTarget, wayPoint);
        // Move to that point.
        MovementCalculationInput(SpeedModifierEnum.Full_Sails);
    }

    private void MovementCalculationInput(SpeedModifierEnum speed)
    {
        if (_currentWaypoint == null) return;
        // Calc direciton to waypoint
        Vector3 directionToWayPoint = (Vector3)_currentWaypoint - transform.position;
        // Calc angle between forward vector and the direction.
        float angleToWayPoint = Vector3.Angle(transform.forward, directionToWayPoint);

        // Determine a speed based on how great the angle is away from the waypoint.
        if (angleToWayPoint > _maxAngleToReduceSpeed)
        {
            _inputManager.MovementInput(_movementData.GetTurnSpeedEasePoint);
        }
        else
        {
            _inputManager.MovementInput(speed);
        }

        // Determine the direction of the next way point and the smallest angle to deviat from that angle.
        _inputManager.Rotation((Vector3)_currentWaypoint, transform.forward);
    }

    private State CheckRange()
    {
        if (!_currentTarget)
        {
            return _followState;
        }
        if (Vector3.Distance(transform.position, _currentTarget.position) > _structuralAttack.GetAmmoData.GetMaxRange)
        {
            _currentTarget = null;
            UnRegisterToTargetHealthComponenets();
            return _followState;
        } else
        {
            return this;
        }
    }

    private void CalculatedDesiredAttack()
    {
        if (!_currentTarget)
        {
            return;
        }
        // Weights are weight + targetHealth + own structural health (longer range is affected more).
        _structuralAttackDesire = (_structuralTargetWeight + _targetStructuralHealth) + (1f - _structuralHealth);
        _mobilityAttackDesire = (_mobilityTargetWeight + _targetMobilityHealth) + (1f - _structuralHealth/2f);
        _corporealAttackDesire = (_corporealTargetWeight + _targetCorporealHealth) + (1f - _structuralHealth/3f);
        // Here we decide what attack type we are going to be doing.
        // This only changes when one weight becomes higher than other weights, or if the current health is equal to 0.
        if (_chosenAttack.Equals(default(AttackTypesStruct)) || _chosenAttack.DesireBeforeChange < Mathf.Max(_structuralAttackDesire, _mobilityAttackDesire, _corporealAttackDesire)
            || _chosenAttack.DesireBeforeChange - _currentAttackTypeContinuationWeight <= 0f)
        {
            if (_structuralAttackDesire >= _mobilityAttackDesire || _structuralAttackDesire >= _corporealAttackDesire)
            {
                _chosenAttack = _structuralAttack;
                _chosenAttack.DesireBeforeChange = _structuralAttackDesire + _currentAttackTypeContinuationWeight;
            }
            if (_mobilityAttackDesire >= _structuralAttackDesire || _mobilityAttackDesire >= _corporealAttackDesire)
            {
                _chosenAttack = _mobiltiyAttack;
                _chosenAttack.DesireBeforeChange = _mobilityAttackDesire + _currentAttackTypeContinuationWeight;
            }
            if (_corporealAttackDesire >= _structuralAttackDesire || _corporealAttackDesire >= _mobilityAttackDesire)
            {
                _chosenAttack = _corporealAttack;
                _chosenAttack.DesireBeforeChange = _corporealAttackDesire + _currentAttackTypeContinuationWeight;
            }
        }
        return;
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
