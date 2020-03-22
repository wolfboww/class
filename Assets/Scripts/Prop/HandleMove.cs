using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleMove : MonoBehaviour
{
    public struct HandleStatus
    {
        public bool isHorizontal;
        public int isPlus;
    }

    public HandleStatus handle;
    public bool isHor;

    private Vector3[] boundary = new Vector3[2];
    private Vector3 dir = Vector3.zero;
    private float speed = 2;

    // Start is called before the first frame update
    void Start()
    {
        handle.isHorizontal = isHor ? true : false;
        handle.isPlus = 0;

        boundary[0] = transform.Find("Boundary").GetChild(0).position;
        boundary[1] = transform.Find("Boundary").GetChild(1).position;
    }

    // Update is called once per frame
    void Update()
    {
        if (handle.isPlus.Equals(0))
            return;

        if (!IfBullet.bemask)
            handle.isPlus = 0;

        if (handle.isHorizontal)
        {
            if (transform.position.x < boundary[0].x)
                dir = handle.isPlus > 0 ? Vector3.right : Vector3.zero;
            else if (transform.position.x > boundary[1].x)
                dir = handle.isPlus > 0 ? Vector3.zero : Vector3.left;
            else
                dir = handle.isPlus > 0 ? Vector3.right : Vector3.left;
        }
        else
        {
            if (transform.position.y < boundary[1].y)
                dir = handle.isPlus > 0 ? Vector3.up : Vector3.zero;
            else if (transform.position.y > boundary[0].y)
                dir = handle.isPlus > 0 ? Vector3.zero : Vector3.down;
            else
                dir = handle.isPlus > 0 ? Vector3.up : Vector3.down;
        }
        Debug.Log(transform.position.x + "   " + boundary[0].x);
        transform.position = Vector2.Lerp(transform.position, transform.position + dir, Time.deltaTime * speed);
    }
}
