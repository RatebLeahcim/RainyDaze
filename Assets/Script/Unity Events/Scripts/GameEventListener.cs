using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    //Event and Response
    [SerializeField] private GameEvent m_event;
    [SerializeField] private UnityEvent m_response;

    #region Manage listener
    private void OnEnable()
    {
        m_event.RegisterListener(this);
    }
    private void OnDisable()
    {
        m_event.UnregisterListener(this);
    }

    public void OnEventRaised()
    {
        m_response.Invoke();
    }
    #endregion
}
