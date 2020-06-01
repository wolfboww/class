﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CubeManager : MonoBehaviour
{
    [HideInInspector]
    public Vector3 freezePos;

    private Transform limit;
    private Transform bottom;
    private bool canMove = true;
    private bool canHit = true;

    private float H;
    private float moveSpeed;
    private float moveTimer = 0;
    private float rayhitY;
    private float initialX;
    private float initialBottomY;

    private void Awake()
    {
        limit = transform.parent.parent.Find("BGLimit");
        bottom = transform.Find("Bottom");
        freezePos = transform.position;
        initialX = transform.position.x;
        initialBottomY = bottom.localPosition.y;
        moveSpeed = transform.GetComponentInParent<CubeMove>().moveSpeed;
    }

    void OnEnable()
    {
        RayHitY();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GameController.isRevive)
        {
            moveTimer = 0;
            canHit = true;
            if (transform.GetComponent<PolygonCollider2D>())
                transform.GetComponent<PolygonCollider2D>().enabled = true;
            transform.Find("Collision").GetComponent<PolygonCollider2D>().isTrigger = true;
            freezePos.x = initialX;
            bottom.localPosition = new Vector3(bottom.localPosition.x, initialBottomY);
            RayHitY();
            this.enabled = false;
        }

        if (!canHit)
            return;
        if (Mathf.Abs(transform.position.y - rayhitY) > 0.1f)
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(freezePos.x, rayhitY), 0.05f);
        else
        {
            canHit = false;
            transform.GetComponentInParent<CubeMove>().down = true;
            if (transform.GetComponent<PolygonCollider2D>())
                transform.GetComponent<PolygonCollider2D>().enabled = false;
        }

        if (!IfBullet.bemask)
            return;
        if (transform.Find("hatPos"))
        {
            H = Input.GetAxisRaw("Horizontal");
            if (transform.Find("hatPos").childCount > 0 && !H.Equals(0))
            {
                if (H > 0)
                {
                    if (RayHitX(transform.Find("Right").position))
                        return;
                }
                else
                {
                    if (RayHitX(transform.Find("Left").position))
                        return;
                    if (RayHitX(transform.Find("Middle").position))
                        return;
                }
                if (canMove)
                {
                    if (Mathf.Abs(transform.Find("Left").position.x - limit.Find("Left").position.x) < 0.1f || Mathf.Abs(transform.Find("Right").position.x - limit.Find("Right").position.x) < 0.1f)
                        return;

                    canMove = false;
                    transform.DOMoveX(transform.position.x + H * moveSpeed, 0.1f).OnComplete(() => RayHitY());
                    freezePos.x = transform.position.x + H * moveSpeed;
                }
            }
        }

        if (!canMove)
        {
            moveTimer += Time.deltaTime;
            if (moveTimer > 0.5f)
            {
                canMove = true;
                moveTimer = 0;
            }
        }
    }

    private bool RayHitX(Vector3 start)
    {
        if (Physics2D.Raycast(start, H > 0 ? Vector3.right : Vector3.left, H, (1 << 8) | (1 << 12)))
            return true;

        return false;
    }
    private void RayHitY()
    {
        float bottomY = GetComponent<PolygonCollider2D>() ? GetComponent<PolygonCollider2D>().bounds.min.y :
            GetComponent<BoxCollider2D>().bounds.min.y;

        if (Physics2D.Raycast(bottom.position, Vector3.down, (1 << 8) | (1 << 12)))
        {
            RaycastHit2D hit = Physics2D.Raycast(bottom.position, Vector3.down, (1 << 8) | (1 << 12));
            rayhitY = hit.point.y + Mathf.Abs(bottomY - transform.position.y);
        }
    }

}
