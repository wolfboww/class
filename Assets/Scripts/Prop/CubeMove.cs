using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CubeMove : MonoBehaviour
{
    public float moveSpeed;
    public static int downIndex;

    private Transform[] child;
    private float moveTimer = 0;
    private float H;
    private bool canMove = true;
    private bool down = false;

    // Start is called before the first frame update
    void Start()
    {
        child = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
            child[i] = transform.GetChild(i);
        downIndex = transform.childCount - 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.isRevive)
        {
            down = false;
            moveTimer = 0;
        }

        if (down)
        {
            down = false;
            child[downIndex].GetComponent<CubeManager>().enabled = true;
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
                        if (Rayhit(item.Find("Middle").position))
                            return;
                    }
                    if (canMove)
                    {
                        canMove = false;
                        item.Translate(Vector3.right * H * moveSpeed);
                        item.GetComponent<CubeManager>().freezePos.x = item.position.x + H * moveSpeed;
                    }
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

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag.Contains("Plane"))
            collision.GetComponentInChildren<PolygonCollider2D>().isTrigger = false;
    }
}
