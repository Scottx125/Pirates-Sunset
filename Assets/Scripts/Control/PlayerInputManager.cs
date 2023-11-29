using System;
using NUnit.Framework;
using PirateGame.Moving;
using UnityEngine;
using System.Collections.Generic;

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
        private GameObject _optionsMenu;

        private IGetData _systemSettingGetData;
        private bool _acceptGameplayInput = true;
        private bool _mouseVisible;

        public void SetGameplayInput(bool x) => _acceptGameplayInput = x;
        public void Setup(MovementManager movementManager, ICameraInterfaces cameraInterfaces, IFireCannons fireCannons, IChangeAmmo changeAmmo)
        {
            if (_movementManager == null) _movementManager = movementManager;
            if (_cameraInterfaces == null) _cameraInterfaces = cameraInterfaces;
            if (_fireCannons == null) _fireCannons = fireCannons;
            if (_changeAmmo == null) _changeAmmo = changeAmmo;
        }

        private void Start()
        {
            _systemSettingGetData = SettingsSystem.Instance;
        }


        private void Update(){
            Input();
            EnableDisableInputsViaMouse();
            CameraState();
            ChangeAmmo();
            Fire();
        }

        private void ChangeAmmo()
        {
            if (UnityEngine.Input.mouseScrollDelta.y == 0) return;
            if (UnityEngine.Input.mouseScrollDelta.y < 0) _changeAmmo.ChangeAmmoType(null, false);
            if (UnityEngine.Input.mouseScrollDelta.y > 0) _changeAmmo.ChangeAmmoType(null, true);
        }

        // Fire if mouse is not enabled.
        private void Fire()
        {
            if (_mouseVisible) return;
            if (_fireCannons == null) return;
            if (!_acceptGameplayInput) return;
            if (UnityEngine.Input.GetMouseButtonDown(0)){
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
        private void EnableDisableInputsViaMouse()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Mouse1))
            {
                EnableDisableInputs();
                SetMouseVisability();
            }
        }

        private void SetMouseVisability()
        {
            if (_mouseVisible)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
            }
            else
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        private void EnableDisableInputs()
        {
            _acceptGameplayInput = !_acceptGameplayInput;
            _mouseVisible = !_mouseVisible;
            SetMouseVisability();
            ResetMovementBools();
        }

        private void ResetMovementBools()
        {
            _movementManager.TurnLeft(false);
            _movementManager.TurnRight(false);
        }

        // Trigger movement.
        private void Input()
        {
            if (UnityEngine.Input.GetKeyDown(_systemSettingGetData.GetKeyCodeData(KeybindMenuEnums.Options))) 
            {
                if (_optionsMenu == null) return;
                _optionsMenu.SetActive(!_optionsMenu.activeSelf);
                EnableDisableInputs();
            }
            if (!_acceptGameplayInput) return;
            if (UnityEngine.Input.GetKey(_systemSettingGetData.GetKeyCodeData(KeybindMenuEnums.Accelerate))){ _movementManager.ChangeSpeed(null, true);}
            if (UnityEngine.Input.GetKey(_systemSettingGetData.GetKeyCodeData(KeybindMenuEnums.Decelerate))) { _movementManager.ChangeSpeed(null, false);}
            if (UnityEngine.Input.GetKeyDown(_systemSettingGetData.GetKeyCodeData(KeybindMenuEnums.Left))) { _movementManager.TurnLeft(true);}
            if (UnityEngine.Input.GetKeyUp(_systemSettingGetData.GetKeyCodeData(KeybindMenuEnums.Left))) { _movementManager.TurnLeft(false);}
            if (UnityEngine.Input.GetKeyDown(_systemSettingGetData.GetKeyCodeData(KeybindMenuEnums.Right))) { _movementManager.TurnRight(true);}
            if (UnityEngine.Input.GetKeyUp(_systemSettingGetData.GetKeyCodeData(KeybindMenuEnums.Right))) { _movementManager.TurnRight(false);}
        }
    }
}