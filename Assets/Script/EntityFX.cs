using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    SpriteRenderer sr;

    [Header("FlashFX info")]
    [SerializeField] Material HitMat;
    [SerializeField] Material GoldMat;
    Material originMat;

    [Header("Ailment colors")]
    [SerializeField] private Color chillColor;
    [SerializeField] private Color[] igniteColor;
    [SerializeField] private Color[] shockColor;


    private void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        originMat = sr.material;     
    }

    private IEnumerator FlashFX()
    {
        sr.material = HitMat;
        yield return new WaitForSeconds(.1f);
        sr.material = originMat;
    }

    private IEnumerator BlingFX()
    {
        sr.material = GoldMat;
        yield return new WaitForSeconds(.1f);
        sr.material = originMat;
    }

    private void RedColourBlink()
    {
        if (sr.color == Color.white)
            sr.color = Color.red;
        else
            sr.color = Color.white;
    }

    private void CancelColorChange()
    {
        CancelInvoke();
        sr.color = Color.white;
    }

    public void IgniteFxFor(float _seconds)
    {
        InvokeRepeating("IgniteColorFx", 0, 0.3f);
        Invoke("CancelColorChange", _seconds);
    }

    private void IgniteColorFx()
    {
        if (sr.color != igniteColor[0])
            sr.color = igniteColor[0];
        else
            sr.color = igniteColor[1];
    }

    private void ChillFxFor();

    private void ShockFxFor();

}
