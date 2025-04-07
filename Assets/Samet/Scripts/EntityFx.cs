using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityFx : MonoBehaviour
{
    SpriteRenderer sr;
    [Header("Flash FX")]
    public Material targetMaterial;
    public float flashDuration;
    private Material originalMaterial;

    private void Start()
    {
        sr= GetComponentInChildren<SpriteRenderer>();
        originalMaterial= sr.material;
    }
    private IEnumerator FlashFx()
    {
        sr.material = targetMaterial;

        yield return new WaitForSeconds(flashDuration);

        sr.material = originalMaterial;
    }
    private void RedColorBlink()
    {
        if (sr.color != Color.white)
            sr.color = Color.white;
        else
            sr.color = Color.red;
    }

    private void CancelRedBlink()
    {
        CancelInvoke();
        sr.color = Color.white;
    }
}
