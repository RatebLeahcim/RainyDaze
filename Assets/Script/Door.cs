using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private Material m_outline;
    [SerializeField] private float m_time = 0.1f;

    private void Awake()
    {
        m_outline = GetComponent<SpriteRenderer>().material;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //Fade in the outline
            StartCoroutine(Outline(m_time, 0.1f, 10f));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //Fade out the outline
            StartCoroutine(Outline(m_time, 0f, 0f));
        }
    }

    private IEnumerator Outline(float time, float opacity, float thickness)
    {
        //Get the current color and thickness of the outline
        Color currentColor = m_outline.GetColor("_Color");
        Color targetColor = new Color(currentColor.r, currentColor.g, currentColor.b, opacity);
        float currentThickness = m_outline.GetFloat("_Border_Length");
        
        //Lerp the color and thickness of the outline
        float elapsedTime = 0f;
        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            m_outline.SetColor("_Color", Color.Lerp(currentColor, targetColor, elapsedTime/time));
            m_outline.SetFloat("_Border_Length", Mathf.Lerp(currentThickness, thickness, elapsedTime / time));
            yield return null;
        }
    }
}
