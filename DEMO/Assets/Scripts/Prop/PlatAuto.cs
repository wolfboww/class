using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatAuto : MonoBehaviour
{
    public enum Dir
    {
        X, Y, GameObject
    }
    public Dir dir = Dir.X;
    public Transform targetObj;

    public float maxDis;
    public float Speed;

    private float dis;
    private Vector2 targetPos;

    // Update is called once per frame
    void Update()
    {
        Distance();

        if (dis < maxDis)
            transform.position = Vector2.Lerp(transform.position, targetPos, Speed * Time.deltaTime);
    }

    private void Distance()
    {
        switch (dir)
        {
            case Dir.X:
                dis = Mathf.Abs(transform.position.x - targetObj.position.x);
                targetPos = new Vector2(transform.position.x, targetObj.position.y);
                break;
            case Dir.Y:
                dis = Mathf.Abs(transform.position.y - targetObj.position.y);
                targetPos = new Vector2(targetObj.position.x, transform.position.y);
                break;
            case Dir.GameObject:
                dis = Vector2.Distance(transform.position, targetObj.position);
                break;
        }
    }
}
