using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
    public void Setup(Transform? mainTarget, Transform? idleTransform, AIInputManager inputManager, SphereCollider sphereCollider, Pathfinder pathfinder, float maxAttackRange, MovementSO movementData)
    {
        _mainTarget = mainTarget;
        _inputManager = inputManager;
        _idleTransform = idleTransform;
        _sphereCollider = sphereCollider;
        _maxAttackRange = maxAttackRange;
        _pathfinder = pathfinder;
        _movementData = movementData;
        if (_sphereCollider == null) return;
        _sphereCollider.radius = _maxAttackRange;
        _elapsedChaseTime = 0f;
        _chasing = false;
    }
    #nullable disable
    private void Update()
    {
        _elapsedChaseTime += Time.deltaTime;
    }

    public override State RunCurrentState()
    {
        MoveToIdlePositionBehaviour();
        // If we have a main target but no ship target then move to main target.
        MoveToMainTargetBehaviour();
        // Move to Ship.
        ChaseShipBehaviour();
        return this;
    }

    private void MoveToIdlePositionBehaviour()
    {
        if (_idleTransform != null && _mainTarget == null && _shipTarget == null){
            if (Vector3.Distance(transform.position, _idleTransform.position) <= 10f){
                MovementCalculationInput(0, false);
            } else {
                _currentWaypoint = _pathfinder.PathToTarget(_idleTransform);
                MovementCalculationInput(4, true);
            }
        }
    }

    private void MoveToMainTargetBehaviour()
    {
        if (_mainTarget != null && _shipTarget == null)
        {
            _currentWaypoint = _pathfinder.PathToTarget(_mainTarget);
            MovementCalculationInput(4, true);
            Attack(_mainTarget);
        }
    }

    private void ChaseShipBehaviour()
    {
        if (_shipTarget != null && Vector3.Distance(transform.position, _shipTarget.position) <= _maxAttackRange)
        {
            Attack(_shipTarget);
        } // If the ship is a target but is out of range chase.
        else if (_shipTarget != null && Vector3.Distance(transform.position, _shipTarget.position) > _maxAttackRange)
        {
            // Try to get back into range.
            _currentWaypoint = _pathfinder.PathToTarget(_shipTarget);
            MovementCalculationInput(4, true);
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
    }

    private State Attack(Transform target)
    {
        // Check if we are in range to attack, if we are return the nextstate.
        if (Vector3.Distance(transform.position, target.position) <= _maxAttackRange)
        {
            //attack
            if (_attackBase != null && target == _attackBase)
            {
                return _attackBase;
            }
            if (_attackShip != null && target == _attackShip)
            {
                return _attackShip;
            }
        }
        return this;
    }

    private void MovementCalculationInput(int speed, bool accelerating)
    {
        if (_currentWaypoint == null) return;
         // Calc direciton to waypoint
        Vector3 directionToWayPoint = (Vector3)_currentWaypoint - transform.position;
        // Calc angle between forward vector and the direction.
        float angleToWayPoint = Vector3.Angle(transform.forward, directionToWayPoint);

        // Determine a speed based on how great the angle is away from the waypoint.
        if (angleToWayPoint > _maxAngleToReduceSpeed){
            _inputManager.MovementInput(_movementData.GetTurnSpeedEasePoint, accelerating);
        } else {
            _inputManager.MovementInput(speed, accelerating);
        }
       
        // Determine the direction of the next way point.
        _inputManager.Rotation((Vector3)_currentWaypoint);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Update the ship target if it's in range.
        _shipTarget = other.transform;
    }
}

