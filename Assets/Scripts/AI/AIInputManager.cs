using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PirateGame.Moving;

public class AIInputManager : MonoBehaviour
{
    [SerializeField]
    private MovementManager _movementManager;
    [SerializeField]
    private IFireCannons _fireCannons;

    public void Setup(MovementManager movementManager, IFireCannons fireCannons)
    {
        if (_movementManager == null) _movementManager = movementManager;
        if (_fireCannons == null) _fireCannons = fireCannons;
    }

    public void Fire(CannonPositionEnum direction)
    {
        // Get direction to fire and fire (this will be direction closest to current target which will be passed in)
    }

    // Input a nullable custom state to specify a new speed OR indicate if you want the speed to increase or decrease.
    public void MovementInput(int? customState, bool increaseSpeed)
    {
        _movementManager.ChangeSpeed(customState, increaseSpeed);
    }

    public void Rotation()
    {
        // Rotate left or right.
    }
}
