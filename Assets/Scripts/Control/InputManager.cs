using System;
using PirateGame.Moving;
using UnityEngine;

namespace PirateGame.Control{
    public class PlayerInputManager : MonoBehaviour
    {
        [SerializeField]
        private MovementManager _movementManager;
        [SerializeField]
        private ICameraInterfaces _cameraInterfaces;
        [SerializeField]
        private IFireCannons _fireCannons;

        // Will be used later.
        private KeyCode increaseSail;
        private KeyCode leftRudder;
        private KeyCode rightRudder;
        private KeyCode reefSail;

        private bool _mouseVisible;


        public void Setup(MovementManager movementManager, ICameraInterfaces cameraInterfaces, IFireCannons fireCannons)
        {
            if (_movementManager == null) _movementManager = movementManager;
            if (_cameraInterfaces == null) _cameraInterfaces = cameraInterfaces;
            if (_fireCannons == null) _fireCannons = fireCannons;

            // Use a SO and change it, it will save the data automatically.
        }

        private void Update(){
            MovementInput();
            SetMouseVisability();
            CameraState();
            Fire();
        }
        // Fire if mouse is not enabled.
        private void Fire()
        {
            if (_mouseVisible) return;
            if (_fireCannons == null) return;
            if (Input.GetMouseButtonDown(0)){
                CannonPositionEnum? posEnum = _cameraInterfaces.CalculateFiringPosition();
                if (posEnum == null) return;
                _fireCannons.FireCannons(posEnum.Value);
                Debug.Log(posEnum.Value);
            }

        }
        // Enable/disable camera movement based on mouse visability.
        private void CameraState()
        {
            if (_cameraInterfaces == null) return;
            _cameraInterfaces.IsEnabled(_mouseVisible);
        }
        // Set mouse visability.
        private void SetMouseVisability()
        {
            if (Input.GetMouseButtonDown(1)){
                _mouseVisible = !_mouseVisible;
            }

            if (_mouseVisible){
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
            } else {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
        // Trigger movement.
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