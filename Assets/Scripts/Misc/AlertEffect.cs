using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlertEffect : MonoBehaviour
{
    Image background;
    public Color startColor;
    public Color endColor;

    public float fadeDuration;
    float t;

    public float lifetime = Mathf.Infinity;
    float totalT = 0f;
    
    void Start()
    {
        background = GetComponent<Image>();
    }

    void Update()
    {
        if (t < 1f)
        {
            t += Time.deltaTime / fadeDuration;
            Color lerpedColor = Color.Lerp(startColor, endColor, t);
            background.color = lerpedColor;

            if (t > 1f)
            {
                Color tempColor = startColor;
                startColor = endColor;
                endColor = tempColor;
                t = 0f;
            }
        }

        totalT += Time.deltaTime;
        if (totalT > lifetime)
            Destroy(gameObject);
    }
}
