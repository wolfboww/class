using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed;
    public float desTime;

    private Vector2 dir = Vector2.right;
    private bool col = false;
    private float timer = 0;
    public GameObject prefabpartical;
    void Start()
    {
        dir = transform.parent.parent.localScale.x > 0 ? Vector2.right : Vector2.left;
        transform.parent.DetachChildren();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= desTime)
            col = true;

        if (!col)
            transform.Translate(dir * speed * Time.deltaTime);
        else
            GetComponent<DestroyController>().enabled = true;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Instantiate(prefabpartical,gameObject.transform.position,gameObject.transform.rotation);
        col = true;
    }
}
