using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.AI;
using PirateGame.Helpers;
using System.Linq;

public class MoveToTargetState : State
{
    [SerializeField]
    private State _attackShip;
    [SerializeField]
    private State _attackBase;
    [SerializeField]
    private float _followTime = 5f;
    [SerializeField]
    private float _maxAngleToReduceSpeed = 67.5f;

    // Current target (this is the base not the player)
    private string _targetable;
    private Transform _mainTarget;
    private Transform _shipTarget;
    private Transform _idleTransform;
    private Vector3? _currentWaypoint;
    private AIInputManager _inputManager;
    private SphereCollider _sphereCollider;
    private Pathfinder _pathfinder;
    private float _elapsedChaseTime;
    private bool _chasing;
    private float _maxAttackRange;
    private MovementSO _movementData;
    
    

    #nullable enable
    public void Setup(Transform? mainTarget, Transform? idleTransform, AIInputManager inputManager, SphereCollider sphereCollider, 
        Pathfinder pathfinder, MovementSO movementData, string targetable, List<AmmunitionSO> ammoList)
    {
        _mainTarget = mainTarget;
        _inputManager = inputManager;
        _idleTransform = idleTransform;
        _sphereCollider = sphereCollider;
        _pathfinder = pathfinder;
        _movementData = movementData;
        if (_sphereCollider == null) return;
        _sphereCollider.radius = _maxAttackRange;
        _elapsedChaseTime = 0f;
        _targetable = targetable;
        _chasing = false;
        _maxAttackRange = AIHelpers.GetAmmunitionRangesInOrder(ammoList).Last().GetMaxRange;
    }

#nullable disable
    private void Update()
    {
        _elapsedChaseTime += Time.deltaTime;
    }

    public override State RunCurrentState()
    {
        State state;
        state = MoveToIdlePositionBehaviour();
        // Move to Ship.
        state = ChaseShipBehaviour();
        // If we have a main target but no ship target then move to main target.
        state = MoveToMainTargetBehaviour();
        
        return state;
    }

    private State MoveToIdlePositionBehaviour()
    {
        if (_idleTransform != null && _mainTarget == null && _shipTarget == null){
            if (Vector3.Distance(transform.position, _idleTransform.position) <= 10f){
                MovementCalculationInput(SpeedModifierEnum.Reefed_Sails);
            } else {
                _currentWaypoint = _pathfinder.PathToTarget(_idleTransform);
                MovementCalculationInput(SpeedModifierEnum.Full_Sails);
            }
        }
        return this;
    }

    private State MoveToMainTargetBehaviour()
    {
        if (_mainTarget != null && _shipTarget == null)
        {
            _currentWaypoint = _pathfinder.PathToTarget(_mainTarget);
            MovementCalculationInput(SpeedModifierEnum.Full_Sails);
            return Attack(_mainTarget);
        }
        return this;
    }

    private State ChaseShipBehaviour()
    {
        if (_shipTarget != null && Vector3.Distance(transform.position, _shipTarget.position) <= _maxAttackRange)
        {
            return Attack(_shipTarget);
        } // If the ship is a target but is out of range chase.
        else if (_shipTarget != null && Vector3.Distance(transform.position, _shipTarget.position) > _maxAttackRange)
        {
            // Try to get back into range.
            _currentWaypoint = _pathfinder.PathToTarget(_shipTarget);
            MovementCalculationInput(SpeedModifierEnum.Full_Sails);
            // Initiate chase
            if (_chasing == false)
            {
                _elapsedChaseTime = 0f;
                _chasing = true;
            }
            // When chase time is reached remove target.
            if (_elapsedChaseTime >= _followTime)
            {
                _shipTarget = null;
                _chasing = false;
            }
        }
        return this;
    }

    private State Attack(Transform target)
    {
        // Check if we are in range to attack, if we are return the nextstate.
        if (Vector3.Distance(transform.position, target.position) <= _maxAttackRange)
        {
            //attack
            if (_attackBase != null && target.name == _mainTarget.name)
            {
                return _attackBase;
            }
            if (_attackShip != null && target.name == _shipTarget.name)
            {
                return _attackShip;
            }
        }
        return this;
    }

    private void MovementCalculationInput(SpeedModifierEnum speed)
    {
        if (_currentWaypoint == null) return;
         // Calc direciton to waypoint
        Vector3 directionToWayPoint = (Vector3)_currentWaypoint - transform.position;
        // Calc angle between forward vector and the direction.
        float angleToWayPoint = Vector3.Angle(transform.forward, directionToWayPoint);

        // Determine a speed based on how great the angle is away from the waypoint.
        if (angleToWayPoint > _maxAngleToReduceSpeed){
            _inputManager.MovementInput(_movementData.GetTurnSpeedEasePoint);
        } else {
            _inputManager.MovementInput(speed);
        }
       
        // Determine the direction of the next way point and the smallest angle to deviat from that angle.
        _inputManager.Rotation((Vector3)_currentWaypoint, transform.forward);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Update the ship target if it's in range.
        if (other.tag == _targetable){
            _shipTarget = other.transform;
        }
    }
}

