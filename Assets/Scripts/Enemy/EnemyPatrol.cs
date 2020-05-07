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
    public bool initialDir;
    public bool canPursue;
    public GameObject bullet;
    public float shootCD;
    public float speed;
    public float maxDis;
    public int life;
    [HideInInspector]
    public bool isDead = false;

    private Animator anim;
    private Transform player;
    private Dir moveBack = Dir.left;
    private Vector3 enemyDir;
    private Vector3 pos;
    private Vector3 shootPos;
    private float timer = 0;
    private float pursueDistance = 6;   //追踪距离
    private int shootNum = 0;
    private int maxLife;

    // Start is called before the first frame update
    void Start()
    {
        player = GameController.Instance.player.transform;
        anim = GetComponent<Animator>();
        pos = transform.position;
        maxLife = life;

        if (transform.Find("ShootPos"))
            shootPos = transform.Find("ShootPos").localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (life <= 0)
            StartCoroutine(GameController.Instance.ResetAnim(anim, "Dead"));

        enemyDir = player.position.x > transform.position.x ?
            Vector3.right : Vector3.left;
        if (isDead || IfBullet.bemask)
            return;
        else if (Vector3.Distance(player.position, transform.position) < pursueDistance && RayHit())
        {
            if (canPursue)
                Pursue();
            Shoot();
        }
        else
            Patrol();


        if (GameController.isRevive)
            Revive();

        if (shootNum > 4 || transform.name != "Enemy4")
            return;
        anim.SetFloat("Shoot", shootNum);
    }

    private bool RayHit()
    {
        Vector3 start = GetComponent<BoxCollider2D>().bounds.center;
        start.x = enemyDir == Vector3.left ? GetComponent<BoxCollider2D>().bounds.min.x - 0.5f : GetComponent<BoxCollider2D>().bounds.max.x + 0.5f;
        RaycastHit2D hit = Physics2D.Raycast(start, (player.GetComponent<BoxCollider2D>().bounds.center - start).normalized, 10);
        if (hit.collider != null)
        {
            if (hit.collider.tag == "Plane" && !hit.collider.isTrigger)
                return false;
        }

        return true;
    }

    private void Pursue()
    {
        anim.SetBool("Attack", true);
        transform.Translate(enemyDir * speed * Time.deltaTime);

        if (initialDir)
            GetComponent<SpriteRenderer>().flipX = enemyDir == Vector3.left ? true : false;
        else
            GetComponent<SpriteRenderer>().flipX = enemyDir == Vector3.right ? true : false;
    }

    private void Shoot()
    {
        if (bullet == null)
            return;

        timer += Time.deltaTime;
        if (timer > shootCD)
        {
            timer = 0;
            if (transform.Find("ShootPos"))
            {
                Vector3 pos = transform.Find("ShootPos").localPosition;
                pos.x = GetComponent<SpriteRenderer>().flipX ? -shootPos.x : shootPos.x;
                transform.Find("ShootPos").localPosition = pos;
            }
            GameObject child = Instantiate(bullet, transform.Find("ShootPos"));
            child.GetComponent<BulletController>().dir = GetComponent<SpriteRenderer>().flipX ? Vector3.right : Vector3.left;
            shootNum++;
        }


    }

    private void Patrol()
    {
        anim.SetBool("Attack", maxDis.Equals(0) ? false : true);
        if (initialDir)
            GetComponent<SpriteRenderer>().flipX = dir == Dir.left ? true : false;
        else
            GetComponent<SpriteRenderer>().flipX = dir == Dir.right ? true : false;

        if (maxDis.Equals(0))
            return;

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
        if (Vector3.Distance(transform.position, pos) > maxDis)
        {
            dir = moveBack;
            pos = transform.position;
        }
    }

    public void Revive()
    {
        if (GetComponent<Jumper>())
        {
            if (GetComponent<Jumper>().fromSpawn)
                Destroy(gameObject);
        }
        else
        {
            life = maxLife;
            anim.SetBool("Attack", false);
        }
    }

    public void EnemyDead(int i)
    {
        isDead = i.Equals(0) ? true : false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            if (collision.gameObject.GetComponent<BulletController>())
                if (collision.gameObject.GetComponent<BulletController>().playerBullet)
                    life--;
        }
    }
}
