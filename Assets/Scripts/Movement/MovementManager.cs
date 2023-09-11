using System.Collections.Generic;
using UnityEngine;

namespace PirateGame.Moving{
    public class MovementManager : MonoBehaviour, ICorporealDamageModifier, IStructuralDamageModifier, IMobilityDamageModifier
    {
        [SerializeField]
        Movement _movement;
        [SerializeField]
        MobilityStates _mobilityStates;
        [SerializeField]
        Rotation _rotation;

        MovementSO _movementData;
        public void Setup(MovementSO movementData, ICurrentSpeed uiCurrentSpeed = null){
            _movementData = movementData;
            List<ICurrentSpeed> currentSpeedList = new List<ICurrentSpeed> {_rotation, uiCurrentSpeed};
            _movement.Setup(_movementData, currentSpeedList);
            _mobilityStates.Setup(_movementData, _movement);
            _rotation.Setup(_movementData);
        }
        public void ChangeSpeed(SpeedModifierEnum? speed, bool? increaseSpeed)
        {
            _mobilityStates.ChangeMobilityState(speed, increaseSpeed);
        }
        public void TurnLeft(bool turnBool){
            _rotation.SetLeftTurn(turnBool);
        }
        public void TurnRight(bool turnBool){
            _rotation.SetRightTurn(turnBool);
        }

        public void CorporealDamageModifier(float modifier, string nameOfSender)
        {
            _rotation.SetCorporealDamageModifier(modifier);
        }

        public void StructuralDamageModifier(float modifier, string nameOfSender)
        {
            _movement.SetStructuralDamageModifier(modifier);
        }

        public void MobilityDamageModifier(float modifier, string nameOfSender)
        {
            _movement.SetMobilityDamageModifier(modifier);
        }
    }
}
