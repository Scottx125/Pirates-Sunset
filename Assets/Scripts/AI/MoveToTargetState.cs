using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveToTargetState : State
{
    [SerializeField]
    private float _pathUpdateDelay = .5f;
    [SerializeField]
    private float _chaseRange = 250f;
    [SerializeField]
    private float _attackRange = 150f;
    [SerializeField]
    private float _followTime = 10f;

    // Current target (this is the base not the player)
    private Transform _mainTarget;
    private Transform _shipTarget;
    private AIInputManager _inputManager;
    private NavMeshPath _path;
    private float _elapsedPathTime;
    private float _elapsedChaseTime;
    private bool _chasing;
    private int _currentWaypointIndex = 0;

    // Add firing state here.
    public void Setup(Transform mainTarget, AIInputManager inputManager)
    {
        _mainTarget = mainTarget;
        _inputManager = inputManager;
        _elapsedPathTime = 0f;
        _elapsedChaseTime = 0f;
        _chasing = false;
        PathToTarget(mainTarget);
    }
    private void Update()
    {
        _elapsedPathTime += Time.deltaTime;
        _elapsedChaseTime += Time.deltaTime;
    }
    public override State RunCurrentState(State? previousState)
    {
        // If main != null and ship == null && outside of range move closer.
        if (_mainTarget != null && _shipTarget == null)
        {
            // Move towards target
            MoveToTarget(_mainTarget);

            return this;
        }
        // Move to Ship.
        if (_shipTarget != null && Vector3.Distance(transform.position, _shipTarget.position) <= _chaseRange)
        {
            MoveToTarget(_shipTarget);
            // If in range initiate attack.
            if (Vector3.Distance(transform.position, _shipTarget.position) <= _attackRange)
            {
                //attack
            }
        } else if (_shipTarget != null && Vector3.Distance(transform.position, _shipTarget.position) > _chaseRange)
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
        return this;
    }

    private void MoveToTarget(Transform pos)
    {
        PathToTarget(pos);
        // Max sail towards target position.

    }

    private void PathToTarget(Transform target)
    {
        if (_elapsedPathTime >= _pathUpdateDelay)
        {
            NavMesh.CalculatePath(transform.position, target.position, NavMesh.AllAreas, _path);
            _elapsedPathTime = 0f;
        }
    }
}

// In this state we want to simply get the base location and move towards a certain range (cannon fire range).
// Then we will switch to the fire state.
