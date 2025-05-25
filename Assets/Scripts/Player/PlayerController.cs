using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : Singleton<PlayerController>, IDataPersistence
{
    public bool FacingLeft { get { return facingLeft; } }


    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float dashSpeed = 4f;
    [SerializeField] private TrailRenderer myTrailRenderer;

    public InputSystem_Actions playerControls;
    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator myAnimator;
    private SpriteRenderer mySpriteRender;
    private float startingMoveSpeed;

    private bool facingLeft = false;
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
        startingMoveSpeed = moveSpeed;
    }

    private void OnEnable()
    {
        playerControls.Enable();
        playerControls.Player.Dash.performed += OnDashPerformed;
    }

    private void OnDisable()
    {
        if (playerControls != null)
        {
            playerControls.Player.Dash.performed -= OnDashPerformed;
        }

    }

    private void Update()
    {
        PlayerInput();
    }

    private void FixedUpdate()
    {
        float xPos = playerControls.Player.Move.ReadValue<Vector2>().x;
        AdjustPlayerFacingDirection(xPos);
        Move();
    }

    private void OnDashPerformed(InputAction.CallbackContext context)
    {
        if (context.performed)
            Dash();
    }
    private void PlayerInput()
    {
        movement = playerControls.Player.Move.ReadValue<Vector2>();

        myAnimator.SetFloat("moveX", movement.x);
        myAnimator.SetFloat("moveY", movement.y);
    }

    private void Move()
    {
        rb.MovePosition(rb.position + movement * (moveSpeed * Time.fixedDeltaTime));
    }

    private void AdjustPlayerFacingDirection(float xPos)
    {
        if (xPos < 0)
        {
            mySpriteRender.flipX = true;
            facingLeft = true;
        }
        else if (xPos > 0)
        {
            mySpriteRender.flipX = false;
            facingLeft = false;
        }
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

    public void LoadData(GameData gameData)
    {
        if (SceneManagement.Instance.LastLoadType == LoadType.LoadGame)
        {
            transform.position = gameData.apocalypticWorldData.worldPosition;
            Debug.Log("Player position restored from save.");
        }
        else
        {
            Debug.Log("Skipped restoring player position due to SceneTransition.");
        }
    }

    public void SaveData(ref GameData gameData)
    {
        Debug.Log($"Saving Player Location: {this.gameObject.transform.position}");
       gameData.apocalypticWorldData.InitialiszeApocalypticWorldPositionData(this.gameObject.transform.position);
        gameData.apocalypticWorldData.hasBeenLoadedBefore = true;
    }

}