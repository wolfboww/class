using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyActive : MonoBehaviour
{
    [HideInInspector]
    public bool active;
    [HideInInspector]
    public bool canShoot = true;

    private Animator anim;
    private Transform shootPos;
    private Transform player;
    private float timer = 0;
    private float shootCD;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        shootPos = transform.Find("ShootPos");
        player = GameController.Instance.player.transform;
        shootCD = GetComponent<EnemyPatrol>().shootCD;
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<SpriteRenderer>().DOFade(active ? 1 : 0, 1);
        GetComponent<BoxCollider2D>().isTrigger = !active ? true : false;

        if (active && !anim.GetCurrentAnimatorStateInfo(0).IsName("EnemyAppear"))
            anim.SetTrigger("Appear");
        else if (!active && !anim.GetCurrentAnimatorStateInfo(0).IsName("EnemyDisappear"))
            anim.SetTrigger("Disappear");

        if (!canShoot)
        {
            GetComponent<EnemyPatrol>().shootCD = shootCD + 6;
            timer += Time.deltaTime;
            if (timer >= 5)
            {
                GetComponent<EnemyPatrol>().shootCD = shootCD;
                canShoot = true;
                timer = 0;
            }
        }
        BulletDir();
    }

    public void BulletDir()
    {
        int dir = GetComponent<SpriteRenderer>().flipX ? 1 : -1;
        RaycastHit2D hit45 = Physics2D.Raycast(shootPos.position, new Vector2(dir, -1), 1 << 8);
        RaycastHit2D hit90 = Physics2D.Raycast(shootPos.position, Vector2.down, 1 << 8);

        bool dis = Mathf.Abs(player.position.x - hit45.point.x) <= Mathf.Abs(player.position.x - hit90.point.x);

        shootPos.localEulerAngles = 
            (dis ? new Vector3(0, 0, -35) : new Vector3(0, 0, -70)) * dir;
    }


    private void ResetAnim()
    {
        anim.ResetTrigger("Appear");
        anim.ResetTrigger("Disappear");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            if (collision.gameObject.GetComponent<BulletController>())
                if (collision.gameObject.GetComponent<BulletController>().playerBullet)
                {
                    active = false;
                    canShoot = false;
                }
        }
    }

}
