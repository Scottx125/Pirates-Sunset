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
    private List<Vector3> _waypoints = new List<Vector3>();
    private Transform _mainTarget;
    private Transform _shipTarget;
    private AIInputManager _inputManager;
    private NavMeshPath _path;
    private float _elapsedPathTime;
    private float _elapsedChaseTime;
    private bool _chasing;
    private bool _calculatedPath;
    private int _currentWaypointIndex = 0;
    private Coroutine _calculatingPath;

    // Add firing state here.
    public void Setup(Transform mainTarget, AIInputManager inputManager)
    {
        _mainTarget = mainTarget;
        _inputManager = inputManager;
        _elapsedPathTime = 0f;
        _elapsedChaseTime = 0f;
        _chasing = false;
        _calculatedPath = false;
        _path = new NavMeshPath();
    }
    private void Update()
    {
        _elapsedPathTime += Time.deltaTime;
        _elapsedChaseTime += Time.deltaTime;
    }

    private void NextWaypoint()
    {
        if (_waypoints.Count > 0 && _currentWaypointIndex < _waypoints.Count - 1)
        {
            if (Vector3.Distance(transform.position, _waypoints[_currentWaypointIndex]) < .5f)
            {
                _currentWaypointIndex++;
            }
        }
    }

    public override State RunCurrentState(State? previousState)
    {
        NextWaypoint();
        // If we have a main target but no ship target then move to main target.
        MoveToMainTargetBehaviour();
        // Move to Ship.
        ChaseShipBehaviour();
        return this;
    }

    private void MoveToMainTargetBehaviour()
    {
        if (_mainTarget != null && _shipTarget == null)
        {
            if (_calculatedPath == false)
            {
                PathToTarget(_mainTarget);
                _calculatedPath = true;
            }
            // Move towards target
            MoveToTarget();
        }
    }

    private void ChaseShipBehaviour()
    {
        // PUT IN CHECK FOR COROUTINE TO SEE IF WE CAN RECALCULATE YET
        if (_shipTarget != null && Vector3.Distance(transform.position, _shipTarget.position) <= _chaseRange)
        {
            _calculatedPath = false;
            PathToTarget(_shipTarget);
            MoveToTarget();
            // If in range initiate attack.
            if (Vector3.Distance(transform.position, _shipTarget.position) <= _attackRange)
            {
                //attack
            }
        } // If the ship is a target but is out of range chase.
        else if (_shipTarget != null && Vector3.Distance(transform.position, _shipTarget.position) > _chaseRange)
        {
            _calculatedPath = false;
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

    private void MoveToTarget()
    {
        if (_waypoints.Count == 0) return;
        // Determine a speed,
        _inputManager.MovementInput(4, true);
        // Determine the direction of the next way point.
        _inputManager.Rotation(_waypoints[_currentWaypointIndex]);
    }

    // Calculate new path
    private void PathToTarget(Transform target)
    {
        if (_elapsedPathTime >= _pathUpdateDelay || target == _mainTarget)
        {
            _waypoints.Clear();
            NavMesh.CalculatePath(transform.position, target.position, NavMesh.AllAreas, _path);
            _calculatingPath = StartCoroutine(CalculatePath());
        }
    }
    private IEnumerator CalculatePath()
    {
        yield return new WaitUntil(()=>_path.status == NavMeshPathStatus.PathComplete || _path.status == NavMeshPathStatus.PathInvalid);
        if (_path.status == NavMeshPathStatus.PathInvalid)
        {
            Debug.Log("Path failed.");
            yield break;
        }
        foreach (Vector3 waypoint in _path.corners)
        {
            _waypoints.Add(waypoint);
        }
        _currentWaypointIndex = 0;
        _elapsedPathTime = 0f;
        _calculatingPath = null;
    }
    private void OnTriggerEnter(Collider other)
    {
        _calculatedPath = false;
        _shipTarget = other.transform;
    }
}

// In this state we want to simply get the base location and move towards a certain range (cannon fire range).
// Then we will switch to the fire state.
