using System.Collections.Generic;
using UnityEngine;

public class BH_Skill_Controller : MonoBehaviour
{
    [SerializeField] private float maxSize;
    [SerializeField] private bool canGrow;
    [SerializeField] private float growSpeed;

    [SerializeField] private List<Transform> enemyTarget;

    private void Update()
    {
        if (canGrow)
        {
            transform.localScale = Vector2.Lerp( transform.localScale, new Vector2(maxSize, maxSize), growSpeed);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            enemyTarget.Add(collision.transform);
        }
    }
}
