using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D m_rigidbody;
    [SerializeField] private Vector2 m_velocity;
    [SerializeField] private float m_acceleration;

    void Start()
    {
        m_rigidbody = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HandleMovement()
    {
        if(Input.GetKeyDown(KeyCode.A)) {
            m_velocity.x += m_acceleration;
        }
    }
}
