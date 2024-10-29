using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    SpriteRenderer sr;

    [Header("FlashFX info")]
    [SerializeField] Material HitMat;
    Material originMat;

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

    private void RedColourBlink()
    {
        if (sr.color == Color.white)
            sr.color = Color.red;
        else
            sr.color = Color.white;
    }

    private void CancelRedBlink()
    {
        CancelInvoke();
        sr.color = Color.white;
    }
}
