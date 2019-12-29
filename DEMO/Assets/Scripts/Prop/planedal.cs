using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class planedal : MonoBehaviour
{
    public bool down = false;
    public bool left = false;
    public float downdis = 2;
    public float leftdis = 2;
    public float downspeed = 1;
    public float leftspeed = 1;
    Vector3 downtarget;
    Vector3 lefttarget;
    void Start()
    {
        downtarget = new Vector2(transform.position.x, transform.position.y - downdis);
        lefttarget = new Vector2(transform.position.x - leftdis, transform.position.y);
    }

    void Update()
    {
        if(down)
        {
            transform.position = Vector2.MoveTowards(transform.position, downtarget, downspeed * Time.deltaTime);
        }
        if(left)
        {
            transform.position = Vector2.MoveTowards(transform.position, lefttarget, leftspeed * Time.deltaTime);
        }
    }
}
