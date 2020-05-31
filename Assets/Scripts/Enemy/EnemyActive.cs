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
    private float timer = 0;
    private float a;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        a = GetComponent<SpriteRenderer>().color.a;
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<SpriteRenderer>().DOFade(active ? 1 : 0, 1);
        GetComponent<BoxCollider2D>().isTrigger = !active ? true : false;

        if (active && !anim.GetCurrentAnimatorStateInfo(0).IsName("EnemyAppear"))
            anim.SetTrigger("Appear");
        else if (!active && !anim.GetCurrentAnimatorStateInfo(0).IsName("EnemyDead"))
            anim.SetTrigger("Disappear");


        if (!canShoot)
        {
            GetComponent<EnemyPatrol>().shootCD += 4;
            timer += Time.deltaTime;
            if (timer >= 3)
            {
                GetComponent<EnemyPatrol>().shootCD -= 4;
                canShoot = true;
                timer = 0;
            }
        }
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
