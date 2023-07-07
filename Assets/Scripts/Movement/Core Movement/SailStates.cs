using UnityEngine;
using PirateGame.Helpers;

public class SailStates : MonoBehaviour
{

    // States of control for sails.
    private int _sailState = 0;

    // State change delay.
    private float _sailStateChangeDelay;
    private float _sailStateTimeSinceChanged;

    private ISailStateModifier _sendSailState;

    private float _crewDamageModifier = 1;

    public void Setup(MovementSO movementData, ISailStateModifier sailStateReciever = null)
    {
        // Get movement and tell it to change it's _sailstatemodifier to this.
        _sailStateChangeDelay = movementData.GetSailStateChangeDelay;
        _sendSailState = sailStateReciever;
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
             ModifySailStateByInterface();
             _sailStateTimeSinceChanged = 0f;
        }
    }
    public void SailStateDecrease()
        {
        if (_sailState > 0 && _sailStateTimeSinceChanged >= _sailStateChangeDelay)
        {
            _sailState--;
            ModifySailStateByInterface();
            _sailStateTimeSinceChanged = 0f;
        }
    }

    private void ModifySailStateByInterface()
    {
        if (_sendSailState != null){
        _sendSailState.SailStateModifier(StaticHelpers.GetSailStateEnumValue(_sailState));
        }
    }
}
