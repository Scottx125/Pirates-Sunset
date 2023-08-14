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
    public void ChangeMobilityState(SpeedModifierEnum? speed, bool? increaseSpeed)
    {
        if (_mobilityState < StaticHelpers.GetMobilityStateEnumLength() && _mobilityStateTimeSinceChanged >= _mobilityStateChangeDelay){
            // Bool increase
            if (increaseSpeed != null){
                if ((bool)increaseSpeed){
                    _mobilityState++;
                } else {_mobilityState--;}
            }
            // State Increase
            if (speed != null){
                if (_mobilityState == (int)speed)return;

                if (_mobilityState < (int)speed){
                _mobilityState++;
                }
                if (_mobilityState > (int)speed){
                _mobilityState--;
                }
            }
            Mathf.Clamp(_mobilityState, 0f, StaticHelpers.GetMobilityStateEnumLength() - 1);
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
