using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class ShipAttackBaseState : State
{
    [SerializeField]
    private State _attackShip;

    private string _targetable;
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
        return this;
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
        Vector3.Distance(transform.position, _mainTarget.position) > 0f)
        {
            // We're in range. Turn to face the target and attack.
            _inputManager.MovementInput(SpeedModifierEnum.Reefed_Sails);
            // Calculate target firing position.
            Vector3 targetToShoot = _targetting.Target(_mainTarget, _ammo.GetSpeed);
            // Rotate towards target based on whichever side is closest.
            (Vector3 directionToShoot, CannonPositionEnum cannonsToFire) = DirectionToTurn(targetToShoot);
            // Calc direciton to waypoint
            Vector3 directionToWayPoint = targetToShoot - transform.position;
            // Calc angle between forward vector and the direction.
            float angleToShoot = Vector3.Angle(directionToShoot, directionToWayPoint);
            if (angleToShoot <= 2.5f)
            {
                Debug.Log(cannonsToFire);
            }
        }
    }

    private (Vector3, CannonPositionEnum) DirectionToTurn(Vector3 targetToShoot)
    {
        Vector3 directionToTarget = targetToShoot - transform.position;
        Vector3 cross = Vector3.Cross(transform.forward, directionToTarget);
        if (cross.y >= 0f)
        {
            _inputManager.Rotation(targetToShoot, transform.right);
            return (transform.right, CannonPositionEnum.Right);
        }
        else
        {
            _inputManager.Rotation(targetToShoot, -transform.right);
            return (-transform.right, CannonPositionEnum.Left);
        }
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
