using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemyBullet : MonoBehaviour
{
    public bool isBoss;
    [HideInInspector]
    public Vector3 dir;
    [HideInInspector]
    public bool skate = false;

    private Rigidbody2D rb;
    private Vector3 direction;
    private float speed = 10;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (!isBoss)
            dir = GameController.Instance.player.transform.position;
        rb.velocity = (dir - transform.position).normalized * speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (skate)
            rb.velocity = CastLaser(direction) * speed;
    }

    private Vector3 CastLaser(Vector3 other)
    {
        Vector3 startPoint = transform.position;
        Vector3 dir = (other - startPoint).normalized;

        var hit = Physics2D.Raycast(startPoint, dir);
        dir = Vector3.Reflect(dir, hit.normal);
        return dir;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "TriggerPos" && isBoss)
            Boss01.isTrigger = true;
        if (collision.gameObject != ColliNameManager.Instance.BossSkate || isBoss)
        {
            if (isBoss)
                GetComponent<Animator>().SetTrigger("Dead");
            else
                GetComponent<DestroyController>().enabled = true;
        }
        if (collision.gameObject == ColliNameManager.Instance.BossSkate && !isBoss)
        {
            skate = true;
            direction = collision.GetContact(0).point;
        }
    }
}
