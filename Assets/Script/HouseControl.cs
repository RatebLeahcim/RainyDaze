using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseControl : MonoBehaviour
{
    [SerializeField] public int id;

    [SerializeField] private SpriteRenderer m_exterior;
    [SerializeField] private SpriteRenderer m_interior;

    [SerializeField] private float m_fadeDuration = 1.0f;
    [SerializeField] private bool m_entered = false;

    public void Interact(int id)
    {
        if(id != this.id)
        {
            return;
        }
        if (!m_entered)
        {
            //Debug.Log(m_entered);
            StartCoroutine(FadeIn());
            SceneManagement.Instance.ChangeToHouseCollisionLayer();
            m_entered = true;
        }
        else
        {
            //Debug.Log(m_entered);
            StartCoroutine(FadeOut());
            SceneManagement.Instance.ChangeToCityCollisionLayer();
            m_entered = false;
        }
        
    }

    IEnumerator FadeIn()
    {
        Debug.Log("FadeIn");
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
    IEnumerator FadeOut()
    {
        Debug.Log("FadeOut");
        Color initialColor = m_exterior.color;
        Color targetColor = new Color(initialColor.r, initialColor.g, initialColor.b, 1.0f);

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
