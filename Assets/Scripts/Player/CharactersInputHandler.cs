using System;
using UnityEngine;

namespace Characters.Handlers
{
    public class CharactersInputHandler : MonoBehaviour
    {
        public InputSystem_Actions PlayerControls { get; private set; }

        public event Action OnDashRequested;
        public event Action OnChangeSelectedIndex;

        private void Awake()
        {
            PlayerControls = new InputSystem_Actions();
        }

        private void OnEnable()
        {
            PlayerControls.Player.Enable();

            PlayerControls.Player.Dash.performed += _ => OnDashRequested?.Invoke();
            PlayerControls.UI.Navigate.performed += _ => OnChangeSelectedIndex?.Invoke();
        }

        private void OnDisable()
        {
            PlayerControls.Player.Disable();
        }

        public Vector2 GetMovementInput()
        {
            return PlayerControls.Player.Move.ReadValue<Vector2>();
        }

        public Vector2 GetNavigateInput()
        {
            return PlayerControls.UI.Navigate.ReadValue<Vector2>();
        }
    }
}