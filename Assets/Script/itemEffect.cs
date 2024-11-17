using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Effect ",menuName = "Data/Item Effect")]
public class itemEffect : ScriptableObject
{
    public void UseEffect()
    {
        Debug.Log("Test Succeed!!!");
    }
}
