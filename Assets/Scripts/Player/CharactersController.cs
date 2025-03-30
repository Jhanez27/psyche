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
    private SpriteRenderer mySpriteRender;
    private float startingMoveSpeed;

    private bool isDashing = false;

    protected override void Awake()
    {
        base.Awake();

        playerControls = new InputSystem_Actions();
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        mySpriteRender = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        playerControls.Player.Dash.performed += _ => Dash();
        startingMoveSpeed = moveSpeed;
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void Onsable()
    {
        playerControls.Disable();
    }

    private void Update()
    {
        PlayerInput();
    }

    private void FixedUpdate()
    {
        //Unable to move when Dialogue is Active
        if(DialogueManager.GetInstance().dialogueIsActive) 
        {
            return;
        }
        else
        {
            Move();
        }
    }

    private void PlayerInput()
    {
        //Get the Movement Input from Player
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");

        //Store for Idle Direction
        if(movement.sqrMagnitude > 0.01f)
        {
            lastMovement = movement;
        }

        //Normalize for Consistent Diagonal Movement Speed
        movement.Normalize();

        //Set the Animator Parameters
        myAnimator.SetFloat("MoveX", movement.x);
        myAnimator.SetFloat("MoveY", movement.y);
        myAnimator.SetFloat("LastMoveX", lastMovement.x);
        myAnimator.SetFloat("LastMoveY", lastMovement.y);
        myAnimator.SetFloat("MoveMagnitude", movement.magnitude);
    }

    private void Move()
    {
        rb.MovePosition(rb.position + movement * (moveSpeed * Time.fixedDeltaTime));
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