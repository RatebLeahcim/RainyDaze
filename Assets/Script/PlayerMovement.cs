using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    //--------------------------------------------------------------------------------
    //Serialized Fields
    //--------------------------------------------------------------------------------

    //Rigidbody2D and GroundCheck components
    [SerializeField] private Rigidbody2D m_rigidbody;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    //Physics Variables
    [SerializeField] private float m_gravity = 9.81f;
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
    [SerializeField] private bool m_isJumping;
    [SerializeField] private bool m_isGrounded;

    //Slope Variables
    [SerializeField] private float m_slopeCheckDistance;
    [SerializeField] private float m_maxSlopeAngle = 45f;
    [SerializeField] private bool m_isOnSlope;
    [SerializeField] private bool m_canWalkOnSlope;

    //Contact Filter
    [SerializeField] private ContactFilter2D m_contactFilter;

    //--------------------------------------------------------------------------------
    //Private Variables
    //--------------------------------------------------------------------------------

    private PlayerInput m_playerInput;
    private movementMode m_movementMode;

    private bool m_isFacingRight = true;
    private float m_slopeDownAngle;
    private float m_slopeSideAngle;
    private float m_slopeDownAngleOld;
    private Vector2 m_slopeNormalPerp;

    private float m_originalGravity;

    //--------------------------------------------------------------------------------
    //Properties
    //--------------------------------------------------------------------------------

    public Vector2 Velocity { get { return m_velocity; } set { m_velocity = value; } }
    public float Speed { get { return m_speed; } }
    public float JumpSpeed { get { return m_jumpSpeed; } set { m_jumpSpeed = value; } }
    public bool IsGrounded { get {  return m_isGrounded; } }

    //--------------------------------------------------------------------------------

    private void Awake()
    {
        m_playerInput = new PlayerInput();
        m_rigidbody = GetComponent<Rigidbody2D>();

        m_playerInput.PlayerControl.Movement.started += OnMovement;
        m_playerInput.PlayerControl.Movement.performed += OnMovement;
        m_playerInput.PlayerControl.Movement.canceled += onMovementCancel;
        m_playerInput.PlayerControl.Jump.performed += OnJump;
        m_playerInput.PlayerControl.Jump.canceled += onJumpCancel;
        m_playerInput.PlayerControl.Interact.started += OnInteract;

        m_originalGravity = m_gravity;

        setUpJumpVariables();
    }

    void FixedUpdate()
    {
        CheckSlope();
        HandleJump();
        HandleMovement();
        HandleGravity();
    }

    #region Enable/Disable
    void OnEnable()
    {
        m_playerInput.Enable();

        m_playerInput.PlayerControl.Movement.started += OnMovement;
        m_playerInput.PlayerControl.Movement.performed += OnMovement;
        m_playerInput.PlayerControl.Movement.canceled += onMovementCancel;
        m_playerInput.PlayerControl.Jump.performed += OnJump;
        m_playerInput.PlayerControl.Jump.canceled += onJumpCancel;
        m_playerInput.PlayerControl.Interact.started += OnInteract;

        setUpJumpVariables();
    }
    void OnDisable()
    {
        m_playerInput.Disable();

        m_playerInput.PlayerControl.Movement.started -= OnMovement;
        m_playerInput.PlayerControl.Movement.performed -= OnMovement;
        m_playerInput.PlayerControl.Movement.canceled -= onMovementCancel;
        m_playerInput.PlayerControl.Jump.performed -= OnJump;
        m_playerInput.PlayerControl.Jump.canceled -= onJumpCancel;
        m_playerInput.PlayerControl.Interact.started -= OnInteract;
    }
    #endregion

    #region Input
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

    #region Interact
    void OnInteract(InputAction.CallbackContext context)
    {
        //Interact with the object
        //door.OnInteract();

        Collider2D[] interacbles = new Collider2D[1];
        int num = Physics2D.OverlapCollider(GetComponent<Collider2D>(), m_contactFilter, interacbles);

        if(num <= 0)
        {
            Debug.Log("No Interactable Found");
            return;
        }

        IInteractable interactable = interacbles[0].GetComponent<IInteractable>();
        if (interactable != null)
        {
            Debug.Log("Interactable Found");
            interactable.OnInteract();
        }

        //foreach (Collider2D collider in interacbles)
        //{
        //    Debug.Log(collider.name);
        //}
    }
    #endregion

    #region Handlers
    private void HandleMovement()
    {
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

        //Handles player movement
        if (m_isOnSlope && m_canWalkOnSlope)
        {
            m_rigidbody.velocity = new Vector2(m_velocity.x * m_speed * m_slopeNormalPerp.x, m_velocity.x * m_speed * m_slopeNormalPerp.y);
            m_rigidbody.gravityScale = 0;
        }
        else
        {
            m_rigidbody.velocity = new Vector2(m_velocity.x * m_speed, m_velocity.y);
            m_rigidbody.gravityScale = m_originalGravity;
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
        return Physics2D.OverlapCircle(groundCheck.position, .5f, groundLayer);
    }

    private void CheckSlope()
    {
        CheckSlopeHorizontal(groundCheck.position);
        CheckSlopeVertcial(groundCheck.position);
    }

    private void CheckSlopeVertcial(Vector2 position)
    {
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.down, m_slopeCheckDistance, groundLayer);

        if(hit)
        {
            Debug.DrawRay(hit.point, hit.normal, Color.green);
            Debug.DrawRay(hit.point, m_slopeNormalPerp, Color.red);

            m_slopeNormalPerp = - Vector2.Perpendicular(hit.normal).normalized;
            m_slopeDownAngle = Vector2.Angle(hit.normal, Vector2.up);

            if(m_slopeDownAngle != m_slopeDownAngleOld)
            {
                m_isOnSlope = true;
            }

            m_slopeDownAngleOld = m_slopeDownAngle;
        }

        if(m_slopeDownAngle > m_maxSlopeAngle)
        {
            m_canWalkOnSlope = false;
        }
        else
        {
            m_canWalkOnSlope = true;
        }
    }

    private void CheckSlopeHorizontal(Vector2 position)
    {
        RaycastHit2D front = Physics2D.Raycast(position, transform.right, m_slopeCheckDistance, groundLayer);
        RaycastHit2D back = Physics2D.Raycast(position, -transform.right, m_slopeCheckDistance, groundLayer);

        if(front)
        {
            m_isOnSlope = true;
            m_slopeSideAngle = Vector2.Angle(front.normal, Vector2.up);
        }
        else if (back)
        {
            m_isOnSlope = true;
            m_slopeSideAngle = Vector2.Angle(back.normal, Vector2.up);
        }
        else
        {
            m_isOnSlope = false;
            m_slopeSideAngle = 0.0f;
        }
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
