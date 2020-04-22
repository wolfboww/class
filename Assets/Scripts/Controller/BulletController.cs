using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public bool independentDir = false;
    public float speed;
    public float desTime;
    public bool playerBullet = false;
    public GameObject prefabpartical;

    private Vector2 dir = Vector2.right;
    private bool col = false;
    private float timer = 0;
    private Animator anim;


    void Awake()
    {
        if (independentDir)
            return;

        if (transform.parent)
            dir = transform.parent.parent.localScale.x > 0 ? Vector2.right : Vector2.left;
    }
    void Start()
    {
        Vector3 scale = transform.localScale;
        scale.x *= scale.x > 0 ? 1 : -1;
        transform.localScale = scale;
        anim = GetComponent<Animator>() ? GetComponent<Animator>() : null;
    }


    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= desTime)
            col = true;

        if (!col)
            transform.Translate(dir * speed * Time.deltaTime);
        else
        {
            if (anim != null)
                anim.SetTrigger("Dead");
            else
                GetComponent<DestroyController>().enabled = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (independentDir)
            return;

        col = true;
        if (prefabpartical)
            Instantiate(prefabpartical, gameObject.transform.position, gameObject.transform.rotation);
    }
}
