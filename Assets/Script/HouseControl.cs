using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseControl : MonoBehaviour
{
    [SerializeField] private string m_name;
    [SerializeField] private SpriteRenderer m_exterior;
    [SerializeField] private SpriteRenderer m_interior;

    [SerializeField] private float m_fadeDuration = 1.0f;
    [SerializeField] private bool m_entered = false;

    public void Interact()
    {
        if (!m_entered)
        {
            m_entered = true;
            StartCoroutine(FadeIn());
        }
        else
        {
            m_entered = false;
            StartCoroutine(FadeOut());
        }
        
    }

    IEnumerator FadeIn()
    {
        float elapsedTime = 0.0f;

        while (elapsedTime < m_fadeDuration)
        {
            // Fade in the house
            elapsedTime += Time.deltaTime;
            m_exterior.color = Color.Lerp(Color.clear, Color.white, elapsedTime / m_fadeDuration);
            yield return null;
        }
    }
    IEnumerator FadeOut()
    {
        Color initialColor = m_exterior.color;
        Color targetColor = new Color(initialColor.r, initialColor.g, initialColor.b, 0.0f);

        float elapsedTime = 0.0f;

        while (elapsedTime < m_fadeDuration)
        {
            // Fade in the house
            elapsedTime += Time.deltaTime;
            m_exterior.color = Color.Lerp(initialColor, targetColor, elapsedTime / m_fadeDuration);
            yield return null;
        }
    }
}
