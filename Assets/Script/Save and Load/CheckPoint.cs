using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private Animator anim;
    public string checkpointId;
    public bool activated;


    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    [ContextMenu("Generate checkpoint id")]//给存档点分配ID
    private void GenerateId()
    {
        checkpointId= System.Guid.NewGuid().ToString();
    }

    private void OnTriggerEnter2D(Collider2D collision)//碰撞则激活存档点
    {
        if(collision.GetComponent<Player>() != null)
        {
            ActivateCheckpoint();
        }
    }

    public void ActivateCheckpoint()
    {
        anim.SetBool("Active", true);
        activated = true;   
    }
}

