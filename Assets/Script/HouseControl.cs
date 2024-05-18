using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseControl : MonoBehaviour
{
    [SerializeField] private string m_name;
    [SerializeField] private SpriteRenderer m_exterior;
    [SerializeField] private SpriteRenderer m_interior;

    [SerializeField] private float m_fadeDuration = 1.0f;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Enter();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            Exit();
        }
    }

    public void Enter()
    {
        StartCoroutine(FadeIn());
    }
    
    public void Exit()
    {
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeIn()
    {
        Color initialColor = m_exterior.color;
        Color targetColor = new Color(initialColor.r, initialColor.g, initialColor.b, 1f);

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
