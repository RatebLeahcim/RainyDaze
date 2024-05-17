using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D m_rigidbody;
    //[SerializeField] private float m_maxRunSpeed;
    //[SerializeField] private float m_maxFallSpeed;
    [SerializeField, Range(0f, 1f)] private float m_friction;

    [SerializeField] private Vector2 m_velocity;
    [SerializeField] private float m_speed;
    [SerializeField] private float m_jumpSpeed;

    private PlayerInput m_playerInput;
    private movementMode m_movementMode;

    public Vector2 Velocity { get { return m_velocity; } }
    public float Speed { get { return m_speed; } }
    public float Friction { get { return m_friction; } set { m_friction = value; } }

    private void Awake()
    {
        m_playerInput = new PlayerInput();
        m_rigidbody = GetComponent<Rigidbody2D>();

        m_playerInput.PlayerMovement.Movement.started += OnMovement;
        m_playerInput.PlayerMovement.Movement.performed += OnMovement;
        m_playerInput.PlayerMovement.Movement.canceled += onMovementCancel;
        m_playerInput.PlayerMovement.Jumping.performed += OnJump;
        m_playerInput.PlayerMovement.Jumping.canceled += onJumpCancel;
    }

    void FixedUpdate()
    {
        HandleMovement();
        HandleJump();
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
        //Debug.Log(m_velocity);
        //Debug.Log(m_speed);
        //Debug.Log(m_velocity.x * m_speed);
        ////Debug.Log(context.ReadValue<Vector2>().x);

    }
    void onMovementCancel(InputAction.CallbackContext context)
    {
        m_movementMode = movementMode.IDLE;
        m_velocity = Vector2.zero;
    }

    void OnJump(InputAction.CallbackContext context)
    {
        m_movementMode = movementMode.JUMPING;
        m_velocity.y = m_jumpSpeed;
    }

    void onJumpCancel(InputAction.CallbackContext context)
    {
        m_movementMode = movementMode.FALLING;
        m_velocity.y = 0;
    }
    #endregion

    #region Handlers
    public void HandleMovement()
    {
        m_rigidbody.velocity = m_velocity * m_speed;
    }

    public void HandleJump()
    {
        m_rigidbody.velocity = m_velocity * m_speed;
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
