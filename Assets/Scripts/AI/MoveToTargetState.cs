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
    private Transform _idleTransform;
    private AIInputManager _inputManager;
    private SphereCollider _sphereCollider;
    private NavMeshPath _path;
    private float _elapsedPathTime;
    private float _elapsedChaseTime;
    private bool _chasing;
    private int _currentWaypointIndex = 0;
    private Coroutine _calculatingPath;

    // Add firing state here.
    #nullable enable
    public void Setup(Transform? mainTarget, Transform? idleTransform, AIInputManager inputManager, SphereCollider sphereCollider)
    {
        _mainTarget = mainTarget;
        _inputManager = inputManager;
        _idleTransform = idleTransform;
        _sphereCollider = sphereCollider;
        if (_sphereCollider == null) return;
        _sphereCollider.radius = _chaseRange;
        _elapsedPathTime = 0f;
        _elapsedChaseTime = 0f;
        _chasing = false;
        _path = new NavMeshPath();
    }
    #nullable disable
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
    #nullable enable
    public override State RunCurrentState(State? previousState)
    {
        NextWaypoint();
        // Idle if we have no main target.
        MoveToIdlePositionBehaviour();
        // If we have a main target but no ship target then move to main target.
        MoveToMainTargetBehaviour();
        // Move to Ship.
        ChaseShipBehaviour();
        return this;
    }
    #nullable disable
    private void MoveToIdlePositionBehaviour()
    {
        if (_idleTransform != null && _mainTarget == null && _shipTarget == null){
            if (Vector3.Distance(transform.position, _idleTransform.position) <= 10f){
                MovementCalculationInput(0, false);
            } else {
                PathToTarget(_idleTransform);
                MovementCalculationInput(4, true);
            }
        }
    }

    private void MoveToMainTargetBehaviour()
    {
        if (_mainTarget != null && _shipTarget == null)
        {
            PathToTarget(_mainTarget);
            MovementCalculationInput(4, true);
            Attack(_mainTarget);
        }
    }

    private void ChaseShipBehaviour()
    {
        if (_shipTarget != null && Vector3.Distance(transform.position, _shipTarget.position) <= _chaseRange)
        {
            PathToTarget(_shipTarget);
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
        if (_waypoints.Count == 0) return;
        // Determine a speed,
        _inputManager.MovementInput(speed, accelerating);
        // Determine the direction of the next way point.
        _inputManager.Rotation(_waypoints[_currentWaypointIndex]);
    }

    // Calculate new path
    private void PathToTarget(Transform target)
    {
        if (_elapsedPathTime >= _pathUpdateDelay && _calculatingPath == null)
        {
            _waypoints.Clear();
            NavMesh.CalculatePath(transform.position, target.position, NavMesh.AllAreas, _path);
            _calculatingPath = StartCoroutine(CalculatePath());
        }
    }
    private IEnumerator CalculatePath()
    {
        // Check to see if the path is complete or invalud and then continue.
        yield return new WaitUntil(()=>_path.status == NavMeshPathStatus.PathComplete || _path.status == NavMeshPathStatus.PathInvalid);
        // If the path failed return.
        if (_path.status == NavMeshPathStatus.PathInvalid)
        {
            Debug.Log("Path failed.");
            yield break;
        }
        // If it created a path, add the current path to the waypoints list.
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
        // Update the ship target if it's in range.
        _shipTarget = other.transform;
    }
}

// In this state we want to simply get the base location and move towards a certain range (cannon fire range).
// Then we will switch to the fire state.
