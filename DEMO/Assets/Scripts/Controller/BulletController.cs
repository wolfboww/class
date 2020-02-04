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
    void Awake()
    {
        if (transform.parent)
            dir = transform.parent.parent.localScale.x > 0 ? Vector2.right : Vector2.left;
    }
    void Start()
    {
        Vector3 scale = transform.localScale;
        scale.x *= scale.x > 0 ? 1 : -1;
        transform.localScale = scale;
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
        col = true;
        if (prefabpartical)
            Instantiate(prefabpartical, gameObject.transform.position, gameObject.transform.rotation);
    }
}
