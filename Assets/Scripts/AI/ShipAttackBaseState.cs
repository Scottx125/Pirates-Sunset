using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class ShipAttackBaseState : State
{
    [SerializeField]
    private State _attackShip;
    [SerializeField]
    private float _maxAngleToReduceSpeed = 67.5f;
    [SerializeField][Range(0.1f, 1f)]
    private float _minAttackRangeModifier = 0.75f;

    private string _targetable;
    private Vector3? _currentWaypoint;
    private Transform _shipTarget;
    private Transform _mainTarget;
    private AIInputManager _inputManager;
    private float _maxAttackRange = 0f;
    private MovementSO _movementData;
    private Targetting _targetting;
    private AmmunitionSO _ammo;
    public void Setup(Transform mainTarget ,AIInputManager inputManager, float maxAttackRange, MovementSO movementData, Targetting targetting, AmmunitionSO ammo, string targetable)
    {
        _mainTarget = mainTarget;
        _inputManager = inputManager;
        _maxAttackRange = maxAttackRange;
        _movementData = movementData;
        _targetting = targetting;
        _ammo = ammo;
        _targetable = targetable;
    }

    public override State RunCurrentState()
    {
        DetectShip();
        AttackBase();
        throw new System.NotImplementedException();
    }

    private State DetectShip()
    {
        if (_shipTarget){
            _shipTarget = null;
            return _attackShip;
        }
        return this;
    }

    private void AttackBase()
    {
        if (_attackShip == null && _mainTarget != null && Vector3.Distance(transform.position, _mainTarget.position) < _maxAttackRange &&
        Vector3.Distance(transform.position, _mainTarget.position) > _maxAttackRange * _minAttackRangeModifier){
            // We're in range. Turn to face the target and attack.
            MovementCalculationInput(SpeedModifierEnum.Reefed_Sails);
            // Calculate target firing position.
            Vector3 targetToShoot = _targetting.Target(_mainTarget, _ammo.GetSpeed);
            // Rotate towards target.
            _inputManager.Rotation(targetToShoot, 90f);

            float angleToShootAt = Vector3.Angle(transform.forward, targetToShoot);
            if (angleToShootAt == 90f){
                Debug.Log("FIREEEEEE");
            }
        }
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
        _inputManager.Rotation((Vector3)_currentWaypoint, 5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == _targetable){
            _shipTarget = other.transform;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        // Update the ship target if it's in range.
        _shipTarget = null;
    }
}
