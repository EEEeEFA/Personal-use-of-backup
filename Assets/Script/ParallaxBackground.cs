using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ParallaxBackground : MonoBehaviour
{
    private GameObject cam;
    private float xPosition;
    private float length;
    [SerializeField]private float parallaxEffect;
    void Start()
    {
        //cam = GetComponent<GameObject>();
        cam = GameObject.Find("Main Camera");

        xPosition = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;

    }

    // Update is called once per frame
    void Update()
    {
        float distanceToMove = cam.transform.position.x * parallaxEffect;//背景要移动的距离
        float distanceMoved = cam.transform.position.x * (1 - parallaxEffect);//背景和摄像机的相对位移

        transform.position = new Vector3(xPosition + distanceToMove, transform.position.y);

        if (distanceMoved > length + xPosition)
            xPosition = xPosition + length;
        else if (distanceMoved < xPosition - length)
            xPosition = xPosition - length;
    }
}
