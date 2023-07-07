using UnityEngine;
namespace PirateGame.Moving{
    public class MovementManager : MonoBehaviour, IDamageModifiers
    {
        [SerializeField]
        MovementSO _movementData;
        [SerializeField]
        Movement _movement;
        [SerializeField]
        SailStates _sailStates;
        [SerializeField]
        Rotation _rotation;

        private bool _isSetup;

        private void Start(){
            if (!_isSetup) Setup();
        }
        public void Setup(){
            _movement.Setup(_movementData, _rotation);
            _sailStates.Setup(_movementData, _movement);
            _rotation.Setup(_movementData);

            _isSetup = true;
        }

        public void IncreaseSpeed(){
            _sailStates.SailStateIncrease();
        }
        public void DecreaseSpeed(){
            _sailStates.SailStateDecrease();
        }
        public void TurnLeft(bool turnBool){
            _rotation.SetLeftTurn(turnBool);
        }
        public void TurnRight(bool turnBool){
            _rotation.SetRightTurn(turnBool);
        }

        public void CrewDamageModifier(float modifier)
        {
            _rotation.SetCrewDamageModifier(modifier);
        }

        public void HullDamageModifier(float modifier)
        {
            _movement.SetHullDamageModifier(modifier);
        }

        public void SailDamageModifier(float modifier)
        {
            _movement.SetSailDamageModifier(modifier);
        }
    }
}
