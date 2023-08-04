using UnityEngine;
using PirateGame.Helpers;

public class MobilityStates : MonoBehaviour
{

    // States of control for sails.
    private int _mobilityState = 0;

    // State change delay.
    private float _mobilityStateChangeDelay;
    private float _mobilityStateTimeSinceChanged;

    private IMobilityStateModifier _sendMobilityState;

    private float _corporealDamageModifier = 1;

    public void Setup(MovementSO movementData, IMobilityStateModifier mobilityStateReciever = null)
    {
        // Get movement and tell it to change it's _sailstatemodifier to this.
        _mobilityStateChangeDelay = movementData.GetMobilityStateChangeDelay;
        _sendMobilityState = mobilityStateReciever;
    }

    private void FixedUpdate()
    {
        SailStateTimer();
    }

    // Timer controling the delay between changing sail state.
    private void SailStateTimer()
    {
        if (_mobilityStateTimeSinceChanged >= _mobilityStateChangeDelay / _corporealDamageModifier){
            _mobilityStateTimeSinceChanged = 1f;
            return;
        }
        _mobilityStateTimeSinceChanged += Time.deltaTime;
    }

    // Increase/decrease ship sail state.
    public void MobilityStateIncrease(int? customState)
    {
        if (customState != null)
        {
            CustomSpeedState(customState);
            return;
        }
        if (_mobilityState < StaticHelpers.GetMobilityStateEnumLength() && _mobilityStateTimeSinceChanged >= _mobilityStateChangeDelay){
            _mobilityState++;
             ModifyMobilityStateByInterface();
             _mobilityStateTimeSinceChanged = 0f;
        }
    }
    public void MobilityStateDecrease(int? customState)
    {
        if (customState != null)
        {
            CustomSpeedState(customState);
            return;
        }
        if (_mobilityState > 0 && _mobilityStateTimeSinceChanged >= _mobilityStateChangeDelay)
        {
            _mobilityState--;
            ModifyMobilityStateByInterface();
            _mobilityStateTimeSinceChanged = 0f;
        }
    }

    private void CustomSpeedState(int? customState)
    {
        if (customState <= StaticHelpers.GetMobilityStateEnumLength())
        {
            _mobilityState = (int)customState;
            ModifyMobilityStateByInterface();
            _mobilityStateTimeSinceChanged = 0f;
        }
    }

    private void ModifyMobilityStateByInterface()
    {
        if (_sendMobilityState != null){
        _sendMobilityState.MobilityStateModifier(StaticHelpers.GetMobilityStateEnumValue(_mobilityState));
        }
    }
}
