using System;
using System.Collections;
using System.Collections.Generic;
using PirateGame.Health;
using PirateGame.Helpers;
using PirateGame.Moving;
using UnityEditor;
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
    private float _lockOnTimeForAttack = 2f;
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
    private bool _attacking;
    private float _timeSinceLastAttackWhileInRange = 0f;
    AttackTypesStruct _structuralAttack = new AttackTypesStruct();
    AttackTypesStruct _mobiltiyAttack = new AttackTypesStruct();
    AttackTypesStruct _corporealAttack = new AttackTypesStruct();
    AttackTypesStruct _chosenAttack = new AttackTypesStruct();

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
        Timers();
    }

    private void Timers()
    {
        if (_attacking)
        {
            _timeSinceLastAttackWhileInRange += Time.deltaTime;
        }

    }

    private void AttackBehaviour()
    {
        // Calculate waypoint if it's not done.
        // If we're close to the range waypoint and haven't created a waypoint.
        // Or If we're within max range, time elapsed is greater than the delay and we've not created a waypoint.
        if (Vector3.Distance(_currentTarget.position, transform.position) <= _chosenAttack.GetAmmoData.GetMaxRange - _shipRangeOffset)
        {
            
            // Calc waypoint..
            _attackWaypointPos = CalculateAttackWaypoint();
            _currentWaypoint = _pathfinder.PathToTarget(transform, _attackWaypointPos);

            // Path to attack waypoint
            if (_attackWaypointPos != null && _currentWaypoint != null)
            {
                // Calc attack waypoint.
                // Get position we need to aim for.
                Vector3 shootingOffsetPos = _targetting.Target(_currentTarget, _chosenAttack.GetAmmoData.GetSpeed);
                // Get the angle from the front of the ship to the shooting position.
                float angleToTarget = Vector3.SignedAngle(transform.forward, shootingOffsetPos - transform.position, Vector3.up);

                // Move to waypoint.
                _currentWaypoint = _pathfinder.CheckNextWaypoint();

                if (Mathf.Abs(angleToTarget) > 92.5f)
                {
                    MovementCalculationInput(_movementData.GetTurnSpeedEasePoint);
                }
                if (Mathf.Abs(angleToTarget) < 87.5f)
                {
                    MovementCalculationInput(SpeedModifierEnum.Full_Sails);
                }



                // If we're in a suitable firing angle.
                if (Mathf.Abs(angleToTarget) >= 87.5f && Mathf.Abs(angleToTarget) <= 92.5f)
                {
                    // Get direction to shoot and Fire.
                    CannonPositionEnum directionToShoot = CalcDirectionToShoot(angleToTarget);
                    _inputManager.Fire(directionToShoot, _chosenAttack.GetAmmoData.GetAmmunitionType);
                }
            }
        }
    }
    private Vector3 CalculateAttackWaypoint()
    {
        // Target position to aim for.
        Vector3 shootingOffsetPos = _targetting.Target(_currentTarget, _chosenAttack.GetAmmoData.GetSpeed);
        // Get the angle from the front of the ship to the target.
        float angleFrontToTarget = Vector3.SignedAngle(transform.forward, shootingOffsetPos - transform.position, Vector3.up);
        if (angleFrontToTarget > 0f)
        {
            angleFrontToTarget = angleFrontToTarget - 90f;
        } else
        {
            angleFrontToTarget = angleFrontToTarget + 90f;
        }
        // Get the player's rotation
        Quaternion playerRotation = transform.rotation;

        // Calculate the rotated vector
        float rotationAngle = angleFrontToTarget * Mathf.Deg2Rad; // Convert to radians
        Vector3 rotatedVector = new Vector3(Mathf.Sin(rotationAngle), 0f, Mathf.Cos(rotationAngle));

        // Calculate the desired position
        Vector3 desiredPosition = transform.position + playerRotation * rotatedVector * 20f;

        Debug.DrawLine(transform.position, desiredPosition, Color.green, 10f);
        return desiredPosition;
    }

    private CannonPositionEnum CalcDirectionToShoot(float angle)
    {
        if (angle > 0)
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
        Vector3 wayPoint = shootingOffsetPos - (directionToTarget * (_chosenAttack.GetAmmoData.GetMaxRange - _shipRangeOffset));
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
        if (Vector3.Distance(transform.position, _currentTarget.position) > _structuralAttack.GetAmmoData.GetMaxRange + _shipRangeOffset)
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
        float healthWeight = _structuralHealth * _shipHealthWeight;
        _structuralAttack.WeightValue = (_structuralTargetWeight + _targetStructuralHealth) + (healthWeight / 3f);
        _mobiltiyAttack.WeightValue = (_mobilityTargetWeight + _targetMobilityHealth) + (healthWeight / 2f);
        _corporealAttack.WeightValue = (_corporealTargetWeight + _targetCorporealHealth) + (healthWeight);
        // Update attacks.
        // Here we decide what attack type we are going to be doing.
        // This only changes when one weight becomes higher than other weights, or if the current health is equal to 0.

        if (_chosenAttack == null || _chosenAttack.TotalWeightValue < Mathf.Max(_structuralAttack.TotalWeightValue, _mobiltiyAttack.TotalWeightValue, _corporealAttack.TotalWeightValue)
            || _chosenAttack.TotalWeightValue <= 0f)
        {
            // Reset the selected modifier on whatever current weight is chosen.
            if (_chosenAttack != null)
            {
                _chosenAttack.SelectedModifier = 0f;
            }
            // Decide on attack.
            if (_structuralAttack.WeightValue >= _mobiltiyAttack.WeightValue && _structuralAttack.WeightValue >= _corporealAttack.WeightValue)
            {
                Debug.Log("long");
                _chosenAttack = _structuralAttack;
                _chosenAttack.SelectedModifier = _currentAttackTypeContinuationWeight;
            } else
            if (_mobiltiyAttack.WeightValue >= _structuralAttack.WeightValue && _mobiltiyAttack.WeightValue >= _corporealAttack.WeightValue)
            {
                Debug.Log("medium");
                _chosenAttack = _mobiltiyAttack;
                _chosenAttack.SelectedModifier = _currentAttackTypeContinuationWeight;
            } else
            if (_corporealAttack.WeightValue >= _structuralAttack.WeightValue && _corporealAttack.WeightValue >= _mobiltiyAttack.WeightValue)
            {
                Debug.Log("short");
                _chosenAttack = _corporealAttack;
                _chosenAttack.SelectedModifier = _currentAttackTypeContinuationWeight;
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
