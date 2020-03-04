using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacMan : MonoBehaviour
{
    public enum Dir
    {
        up, down, left, right
    }
    public Transform path;
    public float speed;

    private int pathLength = 18;
    private Dir dir = Dir.right;
    private Transform[] pathPoint;

    private Vector3 originPos;
    private Vector3 targetPos;

    public bool caught = false;

    void Start()
    {
        pathPoint = new Transform[pathLength];
        for (int i = 0; i < pathLength; i++)
        {
            pathPoint[i] = path.GetChild(i);
        }

        originPos = transform.position;
        targetPos = pathPoint[0].position;
    }

    void Update()
    {
        if (pathPoint == null) return;

        MoveDir(dir);

        //if (index < pathPoint.Length)
        //{
        //    if (Vector3.Distance(transform.position, pathPoint[index].position) > 0.1f)
        //        transform.position = Vector3.MoveTowards(transform.position, pathPoint[index].position, speed * Time.deltaTime);
        //    else
        //    {
        //        index++;
        //        originPos = pathPoint[index - 1].position;
        //        targetPos = pathPoint[index].position;
        //    }
        //}
        //else
        //{
        //    index = 0;
        //    originPos = pathPoint[pathPoint.Length - 1].position;
        //    targetPos = pathPoint[index].position;
        //}

        //判断主角被抓住
        caught = GameController.Instance.player.transform.parent == transform.GetChild(0) ? true : false;
    }

    public void MoveDir(Dir dir)
    {
        if (Mathf.Abs(originPos.x - targetPos.x) < 5)
            dir = originPos.y - targetPos.y > 0 ? Dir.down : Dir.up;
        else if (Mathf.Abs(originPos.y - targetPos.y) < 5)
            dir = originPos.x - targetPos.x > 0 ? Dir.left : Dir.right;

        switch (dir)
        {
            case Dir.up:
                transform.GetChild(0).localEulerAngles = new Vector3(0, 0, 90);
                break;
            case Dir.down:
                transform.GetChild(0).localEulerAngles = new Vector3(0, 0, -90);
                break;
            case Dir.left:
                transform.GetChild(0).localEulerAngles = Vector3.zero;
                break;
            case Dir.right:
                transform.GetChild(0).localEulerAngles = Vector3.zero;
                break;
        }
        GetComponentInChildren<SpriteRenderer>().flipX = dir == Dir.left ? true : false;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.tag == "Bean" || collision.transform.tag == "Mask")
            collision.gameObject.SetActive(false);
        else if (collision.transform.tag == "Player")
        {
            if (collision.transform.Find("Mask").GetComponent<SpriteRenderer>().sprite != null)
                collision.transform.SetParent(transform.GetChild(0));
        }
    }
}