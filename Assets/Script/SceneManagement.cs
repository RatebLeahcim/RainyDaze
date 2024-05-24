using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManagement : MonoBehaviour
{
    public static SceneManagement Instance { get; private set; }

    [SerializeField] private GameObject m_player;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        m_player = GameObject.FindGameObjectWithTag("Player").gameObject;
    }
    public void ChangeToCityCollisionLayer()
    {
        m_player.layer = LayerMask.NameToLayer("City");
    }

    public void ChangeToHouseCollisionLayer()
    {
        m_player.layer = LayerMask.NameToLayer("House");
    }
}
