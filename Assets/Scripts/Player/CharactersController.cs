using Characters.Handlers;
using Inventory;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace Characters
{
    public class CharactersController : Singleton<CharactersController>, IDataPersistence
    {
        private CharactersMovementHandler movementHandler;
        private CharactersAnimationHandler animationHandler;

        private bool MovementEnabled = true;

        protected override void Awake()
        {
            base.Awake();

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
            if (movementHandler == null || GamesEventManager.Instance == null) return;

            GamesEventManager.Instance.playerEvents.OnMovementEnabled -= EnableMovement;
            GamesEventManager.Instance.playerEvents.OnMovementDisabled -= DisableMovement;
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
                return;
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
            Debug.Log("Move Enabled");
        }
        public void DisableMovement()
        {
            MovementEnabled = false;
            movementHandler.SetMovement(Vector2.zero);
        }

        public void LoadData(GameData gameData)
        {
            if (gameData.janeWorldData.hasBeenLoadedBefore)
            {
                this.gameObject.transform.position = gameData.janeWorldData.worldPosition;
            }
        }

        public void SaveData(ref GameData gameData)
        {
            gameData.janeWorldData.InitialiszeJaneWorldPositionData(this.gameObject.transform.position);
            gameData.janeWorldData.hasBeenLoadedBefore = true; 
        }
    }
}