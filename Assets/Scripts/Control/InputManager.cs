using System;
using PirateGame.Moving;
using UnityEngine;

namespace PirateGame.Control{
    public class PlayerInputManager : MonoBehaviour
    {
        [SerializeField]
        private MovementManager _movementManager;
        [SerializeField]
        private CameraController _cameraController;

        // Will be used later.
        private KeyCode increaseSail;
        private KeyCode leftRudder;
        private KeyCode rightRudder;
        private KeyCode reefSail;

        private bool _clickEnabled;


        public void Setup(MovementManager movementManager, CameraController cameraController)
        {
            if (_movementManager == null) _movementManager = movementManager;
            if (_cameraController == null) _cameraController = cameraController;

            // Use a SO and change it, it will save the data automatically.
        }

        private void Update(){
            MovementInput();
            EnableLeftClicking();
        }

        private void EnableLeftClicking()
        {
            if (Input.GetMouseButtonDown(1)){
                _clickEnabled = !_clickEnabled;
            }

            if (_clickEnabled){
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
            } else {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                CameraInput();
            }
        }

        private void CameraInput()
        {
            float xInput = Input.GetAxis("Mouse X");
            float yInput = Input.GetAxis("Mouse Y");
            if (_cameraController != null){
                _cameraController.CameraRotation(xInput, yInput);
            }
        }

        private void MovementInput()
        {
            if (Input.GetKey(KeyCode.W)){_movementManager.IncreaseSpeed();}
            if (Input.GetKey(KeyCode.S)){_movementManager.DecreaseSpeed();}
            if (Input.GetKeyDown(KeyCode.A)){_movementManager.TurnLeft(true);}
            if (Input.GetKeyUp(KeyCode.A)){_movementManager.TurnLeft(false);}
            if (Input.GetKeyDown(KeyCode.D)){_movementManager.TurnRight(true);}
            if (Input.GetKeyUp(KeyCode.D)){_movementManager.TurnRight(false);}
        }
    }
}