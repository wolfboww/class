using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CubeMove : MonoBehaviour
{
    public float moveSpeed;
    private Transform[] child;
    private Vector3[] childTarget;
    private float timer = 0;
    private float H;
    private bool canMove = true;
    private bool down = false;

    // Start is called before the first frame update
    void Start()
    {
        child = new Transform[transform.childCount];
        childTarget = new Vector3[child.Length];
        for (int i = 0; i < transform.childCount; i++)
        {
            child[i] = transform.GetChild(i);
            childTarget[i] = child[i].Find("Target").position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.isRevive)
        {
            down = false;
            timer = 0;
        }
        if (down)
            foreach (var item in child)
            {
                if (Vector3.Distance(item.position, childTarget[item.GetSiblingIndex()]) >= 0.05f)
                    item.position = Vector3.MoveTowards(item.position, childTarget[item.GetSiblingIndex()], Time.deltaTime);
            }

        if (!IfBullet.bemask)
            return;
        foreach (Transform item in child)
        {
            if (item.Find("hatPos"))
            {
                H = Input.GetAxisRaw("Horizontal");
                if (item.Find("hatPos").childCount > 0 && !H.Equals(0))
                {
                    if (H > 0)
                    {
                        if (Rayhit(item.Find("Right").position))
                            return;
                    }
                    else
                    {
                        if (Rayhit(item.Find("Left").position))
                            return;
                        if (item.Find("Middle") && Rayhit(item.Find("Middle").position))
                            return;
                    }
                    if (canMove)
                    {
                        canMove = false;
                        item.Translate(Vector3.right * H * moveSpeed);
                    }
                }
            }
        }

        if (!canMove)
        {
            timer += Time.deltaTime;
            if (timer > 0.5f)
            {
                canMove = true;
                timer = 0;
            }
        }
    }

    private bool Rayhit(Vector3 start)
    {
        if (Physics2D.Raycast(start, H > 0 ? Vector3.right : Vector3.left, H, (1 << 8) | (1 << 12)))
            return true;

        return false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            down = true;
    }
}
