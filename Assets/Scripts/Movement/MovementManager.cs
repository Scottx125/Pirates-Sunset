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
        public void Setup(MovementSO movementData){
            _movementData = movementData;
            _movement.Setup(_movementData, _rotation);
            _mobilityStates.Setup(_movementData, _movement);
            _rotation.Setup(_movementData);
        }
        public void ChangeSpeed(SpeedModifierEnum? speed, bool? increaseSpeed)
        {
            _mobilityStates.ChangeMobilityState((SpeedModifierEnum)speed, (bool)increaseSpeed);
        }
        public void TurnLeft(bool turnBool){
            _rotation.SetLeftTurn(turnBool);
        }
        public void TurnRight(bool turnBool){
            _rotation.SetRightTurn(turnBool);
        }

        public void CorporealDamageModifier(float modifier)
        {
            _rotation.SetCorporealDamageModifier(modifier);
        }

        public void StructuralDamageModifier(float modifier)
        {
            _movement.SetStructuralDamageModifier(modifier);
        }

        public void MobilityDamageModifier(float modifier)
        {
            _movement.SetMobilityDamageModifier(modifier);
        }
    }
}
