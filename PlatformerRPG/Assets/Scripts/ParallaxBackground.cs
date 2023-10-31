using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxbackGround : MonoBehaviour
{
    private GameObject cam;
    
    [SerializeField] private float parallaxEffect;
    
    private float xPosition;
    private float length;

    private void Start()
    {
        cam = GameObject.Find("Main Camera");

        length = GetComponent<SpriteRenderer>().bounds.size.x;
        xPosition = transform.position.x;
    }

    private void Update()
    {
        float distanceMoved = cam.transform.position.x * (1 - parallaxEffect); // 벗어났을때의 거리
        float distanceToMove = cam.transform.position.x * parallaxEffect; // 플레이어의 움직임에 따른 움직임

        transform.position = new Vector3(xPosition + distanceToMove, transform.position.y);

        if (distanceMoved > xPosition + length)
        {
            xPosition = xPosition + length;
        }
        else if (distanceMoved < xPosition - length)
        {
            xPosition = xPosition - length;
        }
    }
}
