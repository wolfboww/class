using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CubeMove : MonoBehaviour
{
    private Transform[] child;
    private float moveSpeed;
    private float timer = 0;
    private float H;
    private bool canMove = true;

    // Start is called before the first frame update
    void Start()
    {
        child = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
            child[i] = transform.GetChild(i);
        moveSpeed = child[2].GetComponent<BoxCollider2D>().bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IfBullet.bemask)
            return;
        foreach (Transform item in child)
        {
            H = Input.GetAxisRaw("Horizontal");
            if (item.Find("hatPos"))
            {
                if (item.Find("hatPos").childCount > 0 && !H.Equals(0))
                {
                    if (Rayhit(item.Find("Right").position + Vector3.one * 0.1f) || Rayhit((item.Find("Left").position - Vector3.one * 0.1f)))
                        return;
                    if (canMove)
                    {
                        canMove = false;
                        item.position = Vector3.MoveTowards(item.position, item.position + Vector3.right * H * moveSpeed, 1);
                    }
                }
            }
        }

        if (!canMove)
        {
            timer += Time.deltaTime;
            if (timer > 1)
            {
                canMove = true;
                timer = 0;
            }
        }
    }

    private bool Rayhit(Vector3 start)
    {
        if (Physics2D.Raycast(start, Vector3.right * H, H * moveSpeed, 1 << 8))
            return true;

        return false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

    }
}
