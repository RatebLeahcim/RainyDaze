using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//https://www.youtube.com/watch?v=7_dyDmF0Ktw
//code from "This is GameDev"
[CreateAssetMenu(fileName = "GameEvent", menuName = "Unity Events/Game Event")]
public class GameEvent : ScriptableObject
{
    [SerializeField] private List<GameEventListener> m_listeners = new List<GameEventListener>();

    public void Raise()
    {
        for (int i = 0; i <= m_listeners.Count - 1; i++)
        {
            //try
            //{
                m_listeners[i].OnEventRaised();
            //}
            //catch (System.ArgumentOutOfRangeException)
            //{
            //    Debug.Log(i);
            //}
        }
    }

    //Register listeners
    public void RegisterListener(GameEventListener listener)
    {
        if (!m_listeners.Contains(listener))
        {
            m_listeners.Add(listener);
        }
    }

    //Unregister listeners
    public void UnregisterListener(GameEventListener listener)
    {
        if (m_listeners.Contains(listener))
        {
            m_listeners.Remove(listener);
        }
    }
}
