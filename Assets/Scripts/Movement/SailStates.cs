using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PirateGame.Helpers;

public class SailStates : MonoBehaviour
{

    // States of control for sails.
    private float _sailStateModifier;
    private int _sailState = 0;

    // State change delay.
    private float _sailStateChangeDelay;
    private float _sailStateTimeSinceChanged;

    private float _crewDamageModifier = 1;

    public void Setup(MoverDataStruct movementData)
    {
        // Get movement and tell it to change it's _sailstatemodifier to this.
        _sailStateChangeDelay = movementData.GetSailStateChangeDelay;
    }

    private void FixedUpdate()
    {
        SailStateTimer();
    }

    // Timer controling the delay between changing sail state.
    private void SailStateTimer()
    {
        if (_sailStateTimeSinceChanged >= _sailStateChangeDelay / _crewDamageModifier){
            _sailStateTimeSinceChanged = 1f;
            return;
        }
        _sailStateTimeSinceChanged += Time.deltaTime;
    }

    // Increase/decrease ship sail state.
    public void SailStateIncrease()
    {
        if (_sailState < StaticHelpers.GetSailStateEnumLength() && _sailStateTimeSinceChanged >= _sailStateChangeDelay){
            _sailState++;
             _sailStateModifier = StaticHelpers.GetSailStateEnumValue(_sailState);
             _sailStateTimeSinceChanged = 0f;
        }
    }
    public void SailStateDecrease()
        {
        if (_sailState > 0 && _sailStateTimeSinceChanged >= _sailStateChangeDelay){
            _sailState--;
            _sailStateModifier = StaticHelpers.GetSailStateEnumValue(_sailState);
            _sailStateTimeSinceChanged = 0f;
        }
    }
}
