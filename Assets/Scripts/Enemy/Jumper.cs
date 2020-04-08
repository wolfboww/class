using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumper : MonoBehaviour
{
    [HideInInspector]
    public bool fromSpawn;

    private Animator anim;
    private Rigidbody2D rb;
    private Transform groundCheck;

    private bool isJump;
    private bool OnLand;
    private int life = 1;
    private float speed = 1.5f;
    private float checkRadius = 0.9f;
    private float destroyHeight;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        groundCheck = transform.Find("groundCheck");

        if (fromSpawn)
            destroyHeight = transform.parent.GetComponent<JumperSpawn>().destroyHeight.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        isJump = !Physics2D.OverlapCircle(groundCheck.position, checkRadius, 1 << LayerMask.NameToLayer("Plane"));

        if (fromSpawn)
        {
            anim.SetBool("OnPlane", !isJump && OnLand);
            if (transform.position.y < destroyHeight)
                anim.SetBool("isDead", true);
            if (transform.position.y < destroyHeight)
                Destroy(GetComponent<DropControl>());
        }
        else
            anim.SetBool("OnPlane", !isJump);

        if (life <= 0)
            anim.SetBool("isDead", true);


        switch (anim.GetCurrentAnimatorClipInfo(0)[0].clip.name)
        {
            case "EnemyFall1":
                rb.gravityScale = 0;
                rb.velocity = Vector2.down * speed;
                break;
            case "EnemyAttack1":
                rb.gravityScale = 1;
                AttackDir();
                break;
        }
    }

    private void AttackDir()
    {
        bool isRight = GameController.Instance.player.transform.position.x > transform.position.x ? true : false;
        GetComponent<SpriteRenderer>().flipX = isRight;

        Vector3 dir = isRight ? Vector3.right : Vector3.left;
        transform.Translate(dir * 0.05f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        OnLand = collision.gameObject == ColliNameManager.Instance.HandleLand ? true : false;

        if (collision.gameObject.tag == "Bullet")
            if (collision.gameObject.GetComponent<BulletController>().playerBullet)
                life--;
    }
}
