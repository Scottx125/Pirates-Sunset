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
        [SerializeField]
        private IChangeAmmo _changeAmmo;
        [SerializeField]
        private InputSO _inputSO;


        private bool _acceptGameplayInput = true;
        private bool _mouseVisible;

        public void SetGameplayInput(bool x) => _acceptGameplayInput = x;
        public void Setup(MovementManager movementManager, ICameraInterfaces cameraInterfaces, IFireCannons fireCannons, IChangeAmmo changeAmmo)
        {
            if (_movementManager == null) _movementManager = movementManager;
            if (_cameraInterfaces == null) _cameraInterfaces = cameraInterfaces;
            if (_fireCannons == null) _fireCannons = fireCannons;
            if (_changeAmmo == null) _changeAmmo = changeAmmo;

            // Use a SO and change it, it will save the data automatically.
        }


        private void Update(){
            MovementInput();
            SetMouseVisability();
            CameraState();
            ChangeAmmo();
            Fire();
        }

        private void ChangeAmmo()
        {
            if (Input.mouseScrollDelta.y == 0) return;
            if (Input.mouseScrollDelta.y < 0) _changeAmmo.ChangeAmmoType(null, false);
            if (Input.mouseScrollDelta.y > 0) _changeAmmo.ChangeAmmoType(null, true);
        }

        // Fire if mouse is not enabled.
        private void Fire()
        {
            if (_mouseVisible) return;
            if (_fireCannons == null) return;
            if (!_acceptGameplayInput) return;
            if (Input.GetMouseButtonDown(0)){
                // Calculate the position of the camera and fire in the direction it's looking in.
                CannonPositionEnum? posEnum = _cameraInterfaces.CalculateFiringPosition();
                if (posEnum == null) return;
                _fireCannons.FireCannons(posEnum.Value);
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
            if (Input.GetMouseButtonDown(1) && _acceptGameplayInput) {
                _mouseVisible = !_mouseVisible;
            } else if (!_acceptGameplayInput)
            {
                _mouseVisible = true;
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
            if (!_acceptGameplayInput) return;
            if (Input.GetKey(_inputSO.ForwardKeyCode)){_movementManager.ChangeSpeed(null, true);}
            if (Input.GetKey(_inputSO.BackKeyCode)){_movementManager.ChangeSpeed(null, false);}
            if (Input.GetKeyDown(_inputSO.LeftKeyCode)){_movementManager.TurnLeft(true);}
            if (Input.GetKeyUp(_inputSO.LeftKeyCode)){_movementManager.TurnLeft(false);}
            if (Input.GetKeyDown(_inputSO.RightKeyCode)){_movementManager.TurnRight(true);}
            if (Input.GetKeyUp(_inputSO.RightKeyCode)){_movementManager.TurnRight(false);}
        }
    }
}