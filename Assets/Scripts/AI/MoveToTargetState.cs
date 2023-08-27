using PirateGame.Helpers;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
    [SerializeField]
    private float _maxDistanceForSlowdown = 60f;

    // Current target (this is the base not the player)
    private string _targetable;
    private Transform _mainTarget;
    private Transform _shipTarget;
    private Transform _idleTransform;
    private Vector3? _currentWaypoint;
    private AIInputManager _inputManager;
    private Pathfinder _pathfinder;
    private float _elapsedChaseTime;
    private bool _chasing = false;
    private float _maxAttackRange;
    private MovementSO _movementData;
    State _state;



#nullable enable
    public void Setup(Transform? mainTarget, Transform? idleTransform, AIInputManager inputManager, 
        Pathfinder pathfinder, MovementSO movementData, string targetable, List<AmmunitionSO> ammoList)
    {
        _mainTarget = mainTarget;
        _inputManager = inputManager;
        _idleTransform = idleTransform;
        _pathfinder = pathfinder;
        _movementData = movementData;
        _elapsedChaseTime = 0f;
        _targetable = targetable;
        _chasing = false;
        _maxAttackRange = AIHelpers.GetAmmunitionRangesInOrder(ammoList).Last().GetMaxRange;
    }

#nullable disable
    private void Update()
    {
        if (_chasing == true)
        {
           _elapsedChaseTime += Time.deltaTime;
        }
       
    }

    public override State RunCurrentState()
    {
        MoveToIdlePositionBehaviour();
        // Move to Ship.
        _state = ChaseShipBehaviour();
        if (_state == _attackShip) return _attackShip;
        // If we have a main target but no ship target then move to main target.
        _state = MoveToMainTargetBehaviour();
        if (_state == _attackBase) return _attackBase;
        
        return _state;
    }

    private void MoveToIdlePositionBehaviour()
    {
        if (_idleTransform != null && _mainTarget == null && _shipTarget == null){

            if (Vector3.Distance(transform.position, _idleTransform.position) <= 10f)
            {
                MovementCalculationInput(SpeedModifierEnum.Reefed_Sails);
            } else {
                    _currentWaypoint = _pathfinder.PathToTarget(_idleTransform, null);
                if (_currentWaypoint != null)
                {
                    _currentWaypoint = _pathfinder.CheckNextWaypoint();
                    float distance = Vector3.Distance(transform.position, _idleTransform.position);
                    MovementCalculationInput(SlowDownOnApproach(distance));
                }
            }
        }
    }

    private State MoveToMainTargetBehaviour()
    {
        if (_mainTarget != null && _shipTarget == null)
        {
            // Calc path
            _currentWaypoint = _pathfinder.PathToTarget(_mainTarget, null);
            // If we have a path move to it.
            if (_currentWaypoint != null)
            {
                _currentWaypoint = _pathfinder.CheckNextWaypoint();
                Vector3 offsetDirection = transform.position - _mainTarget.position;
                Vector3 offsetPosition = _mainTarget.position + offsetDirection.normalized * _maxAttackRange;
                float distance = Vector3.Distance(transform.position, offsetPosition);
                MovementCalculationInput(SlowDownOnApproach(distance));
            }
            return Attack(_mainTarget);
        }
        return this;
    }

    private SpeedModifierEnum SlowDownOnApproach(float distance)
    {
        if (distance > _maxDistanceForSlowdown)
        {
            return SpeedModifierEnum.Full_Sails;
        } else
        if (distance > (_maxDistanceForSlowdown * .66f))
        {
            return SpeedModifierEnum.Three_Quater_Sails;
        } else
        if (distance > (_maxDistanceForSlowdown * .33f))
        {
            return SpeedModifierEnum.Half_Sails;
        } else
        {
            return SpeedModifierEnum.Quater_Sails;
        }
    }

    private State ChaseShipBehaviour()
    {
        if (_shipTarget != null && Vector3.Distance(transform.position, _shipTarget.position) < _maxAttackRange)
        {
            return Attack(_shipTarget);
        } // If the ship is a target but is out of range chase.
        else if (_shipTarget != null && Vector3.Distance(transform.position, _shipTarget.position) > _maxAttackRange)
        {

            _currentWaypoint = _pathfinder.PathToTarget(_shipTarget, null);
            // Try to get back into range.
            if (_currentWaypoint != null)
            {
                _currentWaypoint = _pathfinder.CheckNextWaypoint();
                MovementCalculationInput(SlowDownOnApproach(Mathf.Infinity));
            }
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
        if (Vector3.Distance(transform.position, target.position) < _maxAttackRange)
        {
            //attack
            if (_attackBase != null && target.name == _mainTarget.name)
            {
                _currentWaypoint = null;
                return _attackBase;
            }
            if (_attackShip != null && target.name == _shipTarget.name)
            {
                _chasing = false;
                _currentWaypoint = null;
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
        if (angleToWayPoint > _maxAngleToReduceSpeed && speed > _movementData.GetTurnSpeedEasePoint){
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

