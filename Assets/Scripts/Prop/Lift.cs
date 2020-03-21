﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lift : MonoBehaviour
{
    public float speed;

    private Vector3 lastPos;
    private Vector3 newPos;
    private Vector3 targetPos;
    private bool isPlayerOn;

    // Start is called before the first frame update
    void Start()
    {
        lastPos = transform.position;
        newPos = transform.GetChild(0).position;
    }

    // Update is called once per frame
    void Update()
    {
        targetPos = isPlayerOn ? newPos : lastPos;
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && !isPlayerOn)
            isPlayerOn = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && isPlayerOn)
            isPlayerOn = false;
    }
}
