using UnityEngine;
namespace PirateGame.Moving{
    public class MovementManager : MonoBehaviour, ICorporealDamageModifier, IStructuralDamageModifier, IMobilityDamageModifier
    {
        [SerializeField]
        MovementSO _movementData;
        [SerializeField]
        Movement _movement;
        [SerializeField]
        MobilityStates _mobilityStates;
        [SerializeField]
        Rotation _rotation;

        private bool _isSetup;

        private void Start(){
            if (!_isSetup) Setup();
        }
        public void Setup(){
            _movement.Setup(_movementData, _rotation);
            _mobilityStates.Setup(_movementData, _movement);
            _rotation.Setup(_movementData);

            _isSetup = true;
        }

        public void IncreaseSpeed(){
            _mobilityStates.MobilityStateIncrease();
        }
        public void DecreaseSpeed(){
            _mobilityStates.MobilityStateDecrease();
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
