using Characters.Handlers;
using Inventory;
using UnityEngine;

namespace Characters
{
    public class CharactersController : MonoBehaviour
    {
        private InventoryController inventoryController;
        private CharactersMovementHandler movementHandler;
        private CharactersAnimationHandler animationHandler;

        private bool MovementEnabled = true;

        private void Awake()
        {
            inventoryController = GetComponent<InventoryController>();
            movementHandler = GetComponent<CharactersMovementHandler>();
            animationHandler = GetComponent<CharactersAnimationHandler>();
        }

        private void OnEnable()
        {
            GamesEventManager.Instance.playerEvents.OnMovementEnabled += EnableMovement;
            GamesEventManager.Instance.playerEvents.OnMovementDisabled += DisableMovement;
            GamesEventManager.Instance.inputEvents.OnDashPressed += movementHandler.DashPressed;
            GamesEventManager.Instance.inputEvents.OnMovePressed += movementHandler.SetMovement;
        }

        private void OnDisable()
        {
            GamesEventManager.Instance.playerEvents.OnMovementEnabled += EnableMovement;
            GamesEventManager.Instance.playerEvents.OnMovementDisabled += DisableMovement;
            GamesEventManager.Instance.inputEvents.OnDashPressed -= movementHandler.DashPressed;
            GamesEventManager.Instance.inputEvents.OnMovePressed -= movementHandler.SetMovement;
        }

        private void Update()
        {
            UpdateAnimations();
        }

        private void FixedUpdate()
        {
            UpdateMovement();
        }

        private void UpdateMovement() // Responsible for Player Movement
        {
            //Unable to move when Dialogue is Active
            if (!MovementEnabled || !ActiveUIManager.Instance.ActiveUIType.Equals(ActiveUIType.None))
            {
                movementHandler.SetMovement(Vector2.zero);
            }
            
            movementHandler.Move();
        }
        private void UpdateAnimations() // Responsible for Player Animations
        {
            if (!MovementEnabled || !ActiveUIManager.Instance.ActiveUIType.Equals(ActiveUIType.None))
            {
                animationHandler.SetIdle(); //Set the Animator to Idle

                return;
            }

            //Set the Animator Parameters
            animationHandler.SetMovement(movementHandler.Movement, movementHandler.LastMovement);
        }

        public void EnableMovement()
        {
            MovementEnabled = true;
        }

        public void DisableMovement()
        {
            MovementEnabled = false;
            movementHandler.SetMovement(Vector2.zero);
        }
    }
}