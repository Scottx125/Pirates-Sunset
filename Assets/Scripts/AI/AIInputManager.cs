using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PirateGame.Moving;

public class AIInputManager : MonoBehaviour
{
    private MovementManager _movementManager;
    private IFireCannons _fireCannons;
    private IChangeAmmo _changeAmmo;

    public void Setup(MovementManager movementManager, CannonManager cannonManager)
    {
        if (_movementManager == null) _movementManager = movementManager;
        if (_fireCannons == null) _fireCannons = cannonManager;
        if (_changeAmmo == null) _changeAmmo = cannonManager;
    }

    public void Fire(CannonPositionEnum direction, AmmunitionTypeEnum ammoType)
    {
        _changeAmmo.ChangeAmmoType(ammoType, null);
        _fireCannons.FireCannons(direction);
    }

    // Input a nullable custom state to specify a new speed OR indicate if you want the speed to increase or decrease.
    public void MovementInput(SpeedModifierEnum speed)
    {
        _movementManager.ChangeSpeed(speed, null);
    }

    public void Rotation(Vector3 currentWaypoint, Vector3 directionToFace)
    {
        // Calc direciton to waypoint
        Vector3 directionToWayPoint = currentWaypoint - transform.position;
        // Calc angle between forward vector and the direction.
        float angleToWayPoint = Vector3.Angle(directionToFace, directionToWayPoint);
        // If the angle is large enough to warrent turning.
        if (angleToWayPoint > .25f)
        {
            // crossproduct to determine if the angle is left or right of the forward vector.
            Vector3 crossProduct = Vector3.Cross(directionToFace, directionToWayPoint);
            // Turn left or right.
            if (crossProduct.y < 0)
            {
                _movementManager.TurnLeft(true);
                _movementManager.TurnRight(false);
            } else
            {
                _movementManager.TurnLeft(false);
                _movementManager.TurnRight(true);
            }
        } else
        {
            // If the angle is too small don't bother turning at all.
            _movementManager.TurnLeft(false);
            _movementManager.TurnRight(false);
        }
        
    }
    
}
