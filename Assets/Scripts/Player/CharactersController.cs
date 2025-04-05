using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class CharacterController : Singleton<CharacterController>
{

    [Header ("Character Physics")]
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float dashSpeed = 4f;

    [Header ("Character Components")]
    [SerializeField] private TrailRenderer myTrailRenderer;

    public InputSystem_Actions playerControls;
    private Vector2 movement;
    private Vector2 lastMovement;
    private Rigidbody2D rb;
    private Animator myAnimator;
    private float startingMoveSpeed;

    private bool isDashing;
    private bool canMove;

    protected override void Awake()
    {
        base.Awake();

        playerControls = new InputSystem_Actions();
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();

        movement = lastMovement = Vector2.zero;
        startingMoveSpeed = moveSpeed;

        isDashing = false;
        canMove = true;
    }

    private void OnEnable()
    {
        playerControls.Player.Dash.performed += _ => Dash();
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Player.Dash.performed -= _ => Dash();
        playerControls.Disable();
    }

    private void Update()
    {
        PlayerInput();
    }

    private void FixedUpdate()
    {
        //Unable to move when Dialogue is Active
        if(DialogueManager.Instance.DialogueIsActive || !canMove) {
            return;
        }
        else {
            Move();
        }
    }

    private void PlayerInput()
    {
        if(DialogueManager.Instance.DialogueIsActive || !canMove) 
        {
            //Disable Player Movement when Dialogue is Active
            //Set the Animator Parameters to Idle
            movement = Vector2.zero;

            myAnimator.SetFloat("MoveX", 0f);
            myAnimator.SetFloat("MoveY", 0f);
            myAnimator.SetFloat("MoveMagnitude", 0f);
            
            return;
        }

        //Get the Player Movement Input
        movement = playerControls.Player.Move.ReadValue<Vector2>();

        //Store for Idle Direction
        if(movement.sqrMagnitude > 0.01f)
        {
            lastMovement = movement.normalized;
        }

        //Set the Animator Parameters
        myAnimator.SetFloat("MoveX", movement.x);
        myAnimator.SetFloat("MoveY", movement.y);
        myAnimator.SetFloat("LastMoveX", lastMovement.x);
        myAnimator.SetFloat("LastMoveY", lastMovement.y);
        myAnimator.SetFloat("MoveMagnitude", movement.sqrMagnitude);
    }

    private void Move()
    {
        //Move the Character using Rigidbody2D
        rb.MovePosition(rb.position + movement.normalized * (moveSpeed * Time.fixedDeltaTime));
    }

    public void EnableMovement()
    {
        canMove = true;
    }

    public void DisableMovement()
    {
        canMove = false;
    }

    private void Dash()
    {
        if (!isDashing)
        {
            isDashing = true;
            moveSpeed *= dashSpeed;
            myTrailRenderer.emitting = true;
            StartCoroutine(EndDashRoutine());
        }
    }

    private IEnumerator EndDashRoutine()
    {
        float dashTime = .2f;
        float dashCD = .25f;
        yield return new WaitForSeconds(dashTime);
        moveSpeed = startingMoveSpeed;
        myTrailRenderer.emitting = false;
        yield return new WaitForSeconds(dashCD);
        isDashing = false;
    }


}