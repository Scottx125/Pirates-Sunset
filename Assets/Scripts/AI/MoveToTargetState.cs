using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveToTargetState : State
{
    [SerializeField]
    private State _nextState;
    [SerializeField]
    private float _chaseRange = 250f;
    [SerializeField]
    private float _attackRange = 150f;
    [SerializeField]
    private float _followTime = 10f;

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
    
    

    #nullable enable
    public void Setup(Transform? mainTarget, Transform? idleTransform, AIInputManager inputManager, SphereCollider sphereCollider, Pathfinder pathfinder)
    {
        _mainTarget = mainTarget;
        _inputManager = inputManager;
        _idleTransform = idleTransform;
        _sphereCollider = sphereCollider;
        _pathfinder = pathfinder;
        if (_sphereCollider == null) return;
        _sphereCollider.radius = _chaseRange;
        _elapsedChaseTime = 0f;
        _chasing = false;
    }
    #nullable disable
    private void IterateTimers()
    {
        _elapsedChaseTime += Time.deltaTime;
    }

    public override State RunCurrentState()
    {
        IterateTimers();
        // Idle if we have no main target.
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
        if (_shipTarget != null && Vector3.Distance(transform.position, _shipTarget.position) <= _chaseRange)
        {
            _currentWaypoint = _pathfinder.PathToTarget(_shipTarget);
            MovementCalculationInput(4, true);
            // If in range initiate attack.
            Attack(_shipTarget);
        } // If the ship is a target but is out of range chase.
        // We don't need any pathtotarget here as the ship will continue on it's present course.
        else if (_shipTarget != null && Vector3.Distance(transform.position, _shipTarget.position) > _chaseRange)
        {
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
        if (Vector3.Distance(transform.position, target.position) <= _attackRange)
        {
            //attack
            if (_nextState != null)
            {
                return _nextState;
            }
            
        }
        return this;
    }

    private void MovementCalculationInput(int speed, bool accelerating)
    {
        if (_currentWaypoint == null) return;
        // Determine a speed,
        _inputManager.MovementInput(speed, accelerating);
        // Determine the direction of the next way point.
        _inputManager.Rotation((Vector3)_currentWaypoint);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Update the ship target if it's in range.
        _shipTarget = other.transform;
    }
}

// In this state we want to simply get the base location and move towards a certain range (cannon fire range).
// Then we will switch to the fire state.
