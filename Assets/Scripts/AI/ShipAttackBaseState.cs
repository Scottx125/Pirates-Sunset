using UnityEngine;
using PirateGame.Helpers;
using System.Collections.Generic;

public class ShipAttackBaseState : State
{
    [SerializeField]
    private State _attackShip;

    private string _targetable;
    private Transform _shipTarget;
    private Transform _mainTarget;
    private AIInputManager _inputManager;
    private float _maxAttackRange = 0f;
    private Targetting _targetting;
    private AmmunitionSO _topStructuralDamageAmmo;
    State _state;
    public void Setup(Transform mainTarget ,AIInputManager inputManager, Targetting targetting, List<AmmunitionSO> ammoList, string targetable)
    {
        _mainTarget = mainTarget;
        _inputManager = inputManager;
        _topStructuralDamageAmmo = AIHelpers.GetTopDamageOfType(ammoList, DamageTypeEnum.Structural);
        _maxAttackRange = _topStructuralDamageAmmo.GetMaxRange;
        _targetting = targetting;
        _targetable = targetable;
    }

    public override State RunCurrentState()
    {
        _state = DetectShip();
        if (_state == _attackShip) return _state; 
        AttackBase();
        return _state;
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
        if (_shipTarget == null && _mainTarget != null && Vector3.Distance(transform.position, _mainTarget.position) < _maxAttackRange)
        {
            // We're in range. Turn to face the target and attack.
            _inputManager.MovementInput(SpeedModifierEnum.Reefed_Sails);
            // Calculate target firing position.
            Vector3 targetToShoot = _targetting.Target(_mainTarget, _topStructuralDamageAmmo.GetSpeed);
            // Calc direciton to target
            Vector3 directionToTarget = targetToShoot - transform.position;
            // Rotate towards target based on whichever side is closest.
            (Vector3 directionToShoot, CannonPositionEnum cannonsToFire) = DirectionToTurn(targetToShoot, directionToTarget);
            // Calc angle between forward vector and the direction.
            float angleToShoot = Vector3.Angle(directionToShoot, directionToTarget);
            if (angleToShoot <= 2.5f)
            {
                _inputManager.Fire(cannonsToFire, _topStructuralDamageAmmo.GetAmmunitionType);
            }
        }
    }

    private (Vector3, CannonPositionEnum) DirectionToTurn(Vector3 targetToShoot, Vector3 directionToTarget)
    {
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
