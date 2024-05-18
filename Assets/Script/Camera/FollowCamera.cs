using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private Transform m_player;
    private void Awake()
    {
        m_player = GameObject.FindFirstObjectByType<PlayerMovement>().transform;
    }
    private void Update()
    {
        Vector2 player = m_player.position;
        transform.position = new Vector3(player.x, 1, transform.position.z);
    }
}
