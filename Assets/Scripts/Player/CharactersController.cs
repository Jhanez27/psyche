using Characters.Handlers;
using Inventory;
using UnityEngine;

namespace Characters
{
    public class CharactersController : MonoBehaviour
    {
        private InventoryController inventoryController;
        private CharactersMovementHandler movementHandler;
        private CharactersInputHandler inputHandler;
        private CharactersAnimationHandler animationHandler;

        private bool canMove;

        private void Awake()
        {
            inventoryController = GetComponent<InventoryController>();
            movementHandler = GetComponent<CharactersMovementHandler>();
            inputHandler = GetComponent<CharactersInputHandler>();
            animationHandler = GetComponent<CharactersAnimationHandler>();

            canMove = true;
        }

        private void OnEnable()
        {
            inputHandler.OnDashRequested += movementHandler.Dash;
            inputHandler.PlayerControls.Enable();
        }

        private void OnDisable()
        {
            inputHandler.OnDashRequested -= movementHandler.Dash;
            inputHandler.PlayerControls.Disable();
        }

        private void Update()
        {
            PlayerInput();
        }

        private void FixedUpdate()
        {
            PlayerMovement();
        }

        private void PlayerMovement()
        {
            //Unable to move when Dialogue is Active
            if (DialogueManager.Instance.DialogueIsActive || !canMove || inventoryController.InventoryIsActive)
            {
                return;
            }
            else
            {
                movementHandler.Move();
            }
        }
        private void PlayerInput()
        {
            if (DialogueManager.Instance.DialogueIsActive || !canMove || inventoryController.InventoryIsActive)
            {

                movementHandler.SetMovement(Vector2.zero); //Disable Player Movement when Dialogue is Active
                animationHandler.SetIdle(); //Set the Animator to Idle

                return;
            }

            //Get the Player Movement Input
            movementHandler.SetMovement(inputHandler.GetMovementInput());

            //Set the Animator Parameters
            animationHandler.SetMovement(movementHandler.Movement, movementHandler.LastMovement);
        }

        public void EnableMovement()
        {
            canMove = true;
        }

        public void DisableMovement()
        {
            canMove = false;
        }
    }
}