using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private Transform m_player;
    [SerializeField] private float m_cameraDistance = 3f;
    private void Awake()
    {
        m_player = GameObject.FindFirstObjectByType<PlayerMovement>().transform;
        StartCoroutine(CameraFollow());
    }

    private IEnumerator CameraFollow()
    {
        Vector2 player = m_player.position;
        
        transform.position = new Vector3(player.x, 1, transform.position.z);
        yield return null;

        if (player.y > transform.position.y + m_cameraDistance || player.y < transform.position.y - m_cameraDistance)
        {
            transform.position = new Vector3(player.x, player.y, transform.position.z);
            yield return null;
        }
    }
}
