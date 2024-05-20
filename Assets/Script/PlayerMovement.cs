using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    //Rigidbody2D and GroundCheck components
    [SerializeField] private Rigidbody2D m_rigidbody;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    //Physics Variables
    [SerializeField, Range(0f, 1f)] private float m_friction;
    [SerializeField] private float m_gravity = 9.81f;
    [SerializeField] private float m_groundedGravity = 0.5f;
    [SerializeField] private float m_gravityModifer = 1.2f;

    //Walk Variables
    [SerializeField] private Vector2 m_velocity;
    [SerializeField] private float m_speed = 8f;

    //Jump Variables
    [SerializeField] private float m_jumpSpeed = 16f;
    [SerializeField] private float m_initialJumpSpeed;
    [SerializeField] private float m_maxJumpHeight = 1f;
    [SerializeField] private float m_maxJumpTime = 0.5f;
    [SerializeField] private bool m_isJumpedPressed;
    [SerializeField] private bool m_canJump = true;

    //Check Variables
    [SerializeField] private bool m_isGrounded;
    [SerializeField] private bool m_isJumping;
    private bool m_isFacingRight = true;

    private PlayerInput m_playerInput;
    private movementMode m_movementMode;

    //Properties
    public Vector2 Velocity { get { return m_velocity; } set { m_velocity = value; } }
    public float Speed { get { return m_speed; } }
    public float Friction { get { return m_friction; } set { m_friction = value; } }
    public float JumpSpeed { get { return m_jumpSpeed; } set { m_jumpSpeed = value; } }
    public bool IsGrounded { get {  return m_isGrounded; } }

    private void Awake()
    {
        m_playerInput = new PlayerInput();
        m_rigidbody = GetComponent<Rigidbody2D>();

        m_playerInput.PlayerControl.Movement.started += OnMovement;
        m_playerInput.PlayerControl.Movement.performed += OnMovement;
        m_playerInput.PlayerControl.Movement.canceled += onMovementCancel;
        m_playerInput.PlayerControl.Jump.performed += OnJump;
        m_playerInput.PlayerControl.Jump.canceled += onJumpCancel;

        setUpJumpVariables();
    }

    void FixedUpdate()
    {
        HandleJump();
        HandleMovement();
        HandleGravity();
    }

    #region Input
    void OnEnable()
    {
        m_playerInput.Enable();
    }
    void OnDisable()
    {
        m_playerInput.Disable();
    }
    void OnMovement(InputAction.CallbackContext context)
    {
        m_movementMode = movementMode.MOVING;
        m_velocity.x = context.ReadValue<Vector2>().x;
    }
    void onMovementCancel(InputAction.CallbackContext context)
    {
        m_movementMode = movementMode.IDLE;
        m_velocity.x = 0;
    }

    void OnJump(InputAction.CallbackContext context)
    {
        m_movementMode = movementMode.JUMPING;
        m_isJumpedPressed = true;
    }

    void onJumpCancel(InputAction.CallbackContext context)
    {
        m_movementMode = movementMode.FALLING;
        m_isJumpedPressed = false;
    }
    #endregion

    #region Handlers
    private void HandleMovement()
    {
        m_rigidbody.velocity = new Vector2(m_velocity.x * m_speed, m_velocity.y);
        m_isGrounded = CheckGrounded();

        //Handles player face direction
        if(m_isFacingRight && m_velocity.x < 0)
        {
            Flip();
        }
        else if(!m_isFacingRight && m_velocity.x > 0)
        {
            Flip();
        }
    }
    private void HandleJump()
    {
        if(!m_isJumping && m_isGrounded && m_isJumpedPressed && m_canJump)
        {
            m_isJumping = true;
            m_canJump = false;
            m_velocity.y = m_initialJumpSpeed;
        }
        else if(m_isGrounded)
        {
            m_isJumping = false;
            m_canJump = true;
        }
    }
    private void HandleGravity()
    {
        if (m_isGrounded && !m_isJumping)
        {
            m_velocity.y = 0;
        }
        else if (m_velocity.y < 0)
        {
            m_velocity.y -= m_gravity * m_gravityModifer * Time.deltaTime;
        }
        else
        {
            m_velocity.y -= m_gravity * Time.deltaTime;
        }

    }
    #endregion

    #region Helpers
    private bool CheckGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }
    
    private void Flip()
    {
        m_isFacingRight = !m_isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void setUpJumpVariables()
    {
        float timeToApex = m_maxJumpTime / 2;
        m_gravity = (2 * m_maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        m_initialJumpSpeed = (2 * m_maxJumpHeight)/timeToApex;
    }
    #endregion
}
public enum movementMode
{
    MOVING,
    JUMPING,
    FALLING,
    IDLE
}
