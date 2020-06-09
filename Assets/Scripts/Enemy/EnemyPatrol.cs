using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public float pursueDistance = 6;   //追踪距离
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
        {
            anim.ResetTrigger("Revive");
            StartCoroutine(GameController.Instance.ResetAnim(anim, "Dead"));
        }

        enemyDir = player.position.x > transform.position.x ?
            Vector3.right : Vector3.left;
        isDead = anim.GetCurrentAnimatorStateInfo(0).IsName("EnemyDead");

        if (isDead)
            return;
        else if (Vector3.Distance(player.position, transform.position) < pursueDistance && RayHit())
        {
            StartCoroutine(GameController.Instance.Language(transform, "!!!", "!!!"));
            if (canPursue)
                Pursue();
            else
                Patrol();
            Shoot();
        }
        else
            Patrol();

        if (GameController.isRevive)
            Revive();

        if (gameObject.name != "Enemy4")
            return;
        anim.SetFloat("Shoot", life);
    }

    private bool RayHit()
    {
        if (IfBullet.bemask)
            return false;

        Vector3 start = GetComponent<BoxCollider2D>().bounds.center;
        start.x = enemyDir == Vector3.left ? GetComponent<BoxCollider2D>().bounds.min.x - 0.5f : GetComponent<BoxCollider2D>().bounds.max.x + 0.5f;

        int layermask = ~((1 << 10) | (1 << 12) | (1 << 18));   //射线无视bullet和mask层
        if (player.GetComponent<BoxCollider2D>())
        {
            RaycastHit2D hit = Physics2D.Raycast(start, (player.GetComponent<BoxCollider2D>().bounds.center - start).normalized, 10, layermask);

            if (hit.collider != null)
            {
                if (hit.collider.tag.Contains("Player"))
                    return true;
            }
        }

        return false;
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

        if (initialDir)
            GetComponent<SpriteRenderer>().flipX = enemyDir == Vector3.left ? true : false;
        else
            GetComponent<SpriteRenderer>().flipX = enemyDir == Vector3.right ? true : false;

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
            StartCoroutine(GameController.Instance.Language(transform, "***", "%$^"));
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
        if (Mathf.Abs(transform.position.x - pos.x) > maxDis)
        {
            dir = moveBack;
            pos.x = transform.position.x;
        }
    }

    public void Revive()
    {
        life = maxLife;
        if (GetComponent<Jumper>())
        {
            if (GetComponent<Jumper>().fromSpawn)
                Destroy(gameObject);
        }
        else
        {
            anim.SetBool("Attack", false);
        }
    }

    public void EnemyDead(int i)
    {
        isDead = i.Equals(0) ? true : false;
    }

    public void DeadLanguage()
    {
        StartCoroutine(GameController.Instance.Language(transform, "???", "..."));

        if (GetComponent<Jumper>())
            if (GetComponent<Jumper>().fromSpawn)
                return;
        StartCoroutine(GameController.Instance.Language(GameController.Instance.player.transform, "^_^", "･◡･"));
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
