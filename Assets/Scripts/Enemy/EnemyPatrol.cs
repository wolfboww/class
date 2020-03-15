using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public enum Dir
    {
        up, down, left, right
    }
    public Dir dir = Dir.left;
    public float speed;
    public float maxDis;
    public int life;

    private Animator anim;
    private MapManager root;
    private Dir moveBack = Dir.left;
    private Vector3 pos;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        root = transform.root.GetComponent<MapManager>();
        pos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (life <= 0)
        {
            if (Random.Range(0, 1.0f) <= root.dropRate)
                Instantiate(root.collection, transform.position, Quaternion.identity);

            anim.SetTrigger("Dead");
            GetComponent<DestroyController>().enabled = true;
        }
        Patrol();
    }

    private void Patrol()
    {
        switch (dir)
        {
            case Dir.left:
                moveBack = Dir.right;
                transform.Translate(Vector3.left * speed * Time.deltaTime);
                break;
            case Dir.right:
                moveBack = Dir.left;
                transform.Translate(Vector3.right * speed * Time.deltaTime);
                break;
            case Dir.up:
                moveBack = Dir.down;
                transform.Translate(Vector3.up * speed * Time.deltaTime);
                break;
            case Dir.down:
                moveBack = Dir.up;
                transform.Translate(Vector3.down * speed * Time.deltaTime);
                break;
        }

        GetComponent<SpriteRenderer>().flipX = dir == Dir.left ? true : false;
        if (Vector3.Distance(transform.position, pos) > maxDis)
        {
            dir = moveBack;
            pos = transform.position;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            if (collision.gameObject.GetComponent<BulletController>().playerBullet)
                life--;
        }
    }
}
