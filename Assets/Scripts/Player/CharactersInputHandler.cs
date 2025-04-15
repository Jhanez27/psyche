using System;
using UnityEngine;

namespace Characters.Handlers
{
    public class CharactersInputHandler : MonoBehaviour
    {
        public InputSystem_Actions PlayerControls { get; private set; }

        public event Action OnDashRequested;

        private void Awake()
        {
            PlayerControls = new InputSystem_Actions();
        }

        private void OnEnable()
        {
            PlayerControls.Player.Enable();

            PlayerControls.Player.Dash.performed += _ => OnDashRequested?.Invoke();
        }

        private void OnDisable()
        {
            PlayerControls.Player.Disable();
        }

        public Vector2 GetMovementInput()
        {
            return PlayerControls.Player.Move.ReadValue<Vector2>();
        }


    }
}