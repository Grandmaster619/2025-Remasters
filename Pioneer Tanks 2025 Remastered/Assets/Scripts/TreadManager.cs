using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreadManager : MonoBehaviour
{
    public int fadeTime = 10;
    private float fadeAmount = 1f;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        float initialAlphaValue = spriteRenderer.color.a;
        fadeAmount = initialAlphaValue / fadeTime;
        StartCoroutine(Lifetime());
    }

    // Update is called once per frame
    void Update()
    {
        float newAlphaValue = spriteRenderer.color.a - fadeAmount * Time.deltaTime;
        spriteRenderer.color = new Color(255.0f, 255.0f, 255.0f, newAlphaValue);
    }

    IEnumerator Lifetime()
    {
        yield return new WaitForSecondsRealtime(fadeTime + 1);
        Destroy(gameObject);
    }
}
