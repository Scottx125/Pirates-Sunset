using System;
using System.Collections.Generic;
using UnityEngine;

public class CannonManager : MonoBehaviour, ICannonManagerLoaded
{
    [SerializeField]
    private float _loadedRefreshTime = 1f;

    // data for cannon from SO.

    [SerializeField]
    private List<Cannon> _cannons = new List<Cannon>();

    private DamageType currentDamageType = DamageType.Hull;

    private int _leftCannonsTotal;
    private int _rightCannonsTotal;
    private int _leftCannonsReadyToFire;
    private int _rightCannonsReadyToFire;

    public void Setup(){
        CannonsCount();
        // Setup cannons
    }

    private void CannonsCount()
    {
        foreach(Cannon cannon in _cannons){
            if (cannon.GetCannonPosition == CannonPositionEnum.Left){
                _leftCannonsTotal++;
            }
            if (cannon.GetCannonPosition == CannonPositionEnum.Right){
                _rightCannonsTotal++;
            }
        }
    }

    public void ChangeAmmoType(){
        //Iterate through ammo type to pass.
    }

    public void FireCannons(CannonPositionEnum direciton){
        // Iterate through cannon positions and fire.
    }

    public void CannonLoaded(CannonPositionEnum position)
    {
        if (position == CannonPositionEnum.Left){
            _leftCannonsReadyToFire++;
        }
        if (position == CannonPositionEnum.Right){
            _rightCannonsReadyToFire++;
        }
    }
}
