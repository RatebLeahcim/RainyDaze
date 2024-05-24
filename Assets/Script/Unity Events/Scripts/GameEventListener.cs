using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//https://www.youtube.com/watch?v=7_dyDmF0Ktw
//code from "This is GameDev"

public class GameEventListener : MonoBehaviour
{
    //Event and Response
    [SerializeField] private GameEvent m_event;
    [SerializeField] private UnityEvent<int> m_response;

    #region Manage listener
    private void OnEnable()
    {
        m_event.RegisterListener(this);
    }
    private void OnDisable()
    {
        m_event.UnregisterListener(this);
    }

    public void OnEventRaised(int id)
    {
        m_response.Invoke(id);
    }
    #endregion
}
