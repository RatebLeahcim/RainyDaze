using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : ScriptableObject
{
    [SerializeField] private string m_name;
    [SerializeField] private Sprite m_exterior;
    [SerializeField] private Sprite m_interior;

    IEnumerator FadeIn(float endvalue, float duration)
    {
   
        // Fade in the house
    }
    void FadeOut()
    {
        // Fade out the house
    }
}
