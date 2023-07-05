using UnityEngine;
namespace PirateGame.Movement{
    public class MovementManager : MonoBehaviour
    {
            [SerializeField]
            RotationCalc _rotation;
            [SerializeField]
            MovementCalc _movement;
            [SerializeField]
            SailStates _sailStates;

            public void Setup(MoverDataStruct movementData){
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
    }
}
