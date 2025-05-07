using System.Collections;
using UnityEngine;

public class EntityFx : MonoBehaviour
{
    [Header("References")]
    private SpriteRenderer sr;

    [Header("Material Flash FX")]
    public Material targetMaterial;
    public float flashDuration = 0.1f;
    private Material originalMaterial;

    [Header("Color Blink FX")]
    public Color blinkColor = Color.red;
    public float blinkInterval = 0.1f;
    public float totalBlinkTime = 0.4f;
    private Color originalColor;

    private void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();

        if (sr != null)
        {
            originalMaterial = sr.material;
            originalColor = sr.color;
        }
    }

    // ▶️ Call this for a brief material flash (e.g. with glowing white or shader)
    public void Flash()
    {
        if (targetMaterial != null && sr != null)
            StartCoroutine(FlashFx());
    }

    private IEnumerator FlashFx()
    {
        sr.material = targetMaterial;
        yield return new WaitForSeconds(flashDuration);
        sr.material = originalMaterial;
    }

    // ▶️ Call this for red/white blinking tint effect (fallback if no material)
    public void BlinkRed()
    {
        if (sr != null)
            StartCoroutine(BlinkRoutine());
    }

    private IEnumerator BlinkRoutine()
    {
        float timer = 0f;
        bool toggle = false;

        while (timer < totalBlinkTime)
        {
            sr.color = toggle ? blinkColor : originalColor;
            toggle = !toggle;
            yield return new WaitForSeconds(blinkInterval);
            timer += blinkInterval;
        }

        sr.color = originalColor;
    }
}
