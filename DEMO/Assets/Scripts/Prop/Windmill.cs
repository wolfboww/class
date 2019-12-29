using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Windmill : MonoBehaviour
{
    public float dir = 1;               //旋转方向
    public float radius;
    public float rotateSpeed = 10f;
    private Transform[] child;

    // Start is called before the first frame update
    void Start()
    {
        child = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            child[i] = transform.GetChild(i);
            child[i].position = new Vector2(transform.position.x + radius * Mathf.Cos(i * Mathf.PI * 2 / transform.childCount), transform.position.y + radius * Mathf.Sin(i * Mathf.PI * 2 / transform.childCount));
        }
    }

    // Update is called once per frame
    void Update()
    {
        RotateAround(dir > 0 ? Vector3.forward : Vector3.back);
    }

    private void RotateAround(Vector3 dir)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            child[i].RotateAround(transform.position, dir, rotateSpeed * Time.deltaTime);
            child[i].localEulerAngles = Vector3.zero;
        }
    }
}
