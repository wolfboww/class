using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : MonoBehaviour
{
    public GameObject bullet;
    [HideInInspector]
    public float limitY;
    public float speed;

    private Transform weaponPoint;
    private Transform player;
    private Rigidbody2D rb;
    private Animator anim;

    private float timer = 0;
    private float shootCD = 3;
    private int life = 1;

    // Start is called before the first frame update
    void Start()
    {
        weaponPoint = transform.Find("WeaponPoint");
        rb = GetComponent<Rigidbody2D>();
        player = GameController.Instance.player.transform;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > shootCD)
        {
            GameObject eBullet = Instantiate(bullet, weaponPoint.position, Quaternion.identity);
            GameController.Instance.BulletLookAt(eBullet.transform, player.position);
            StartCoroutine(GameController.Instance.Language(transform, "!!!", "%$^"));
            timer = 0;
        }
        else
            timer += Time.deltaTime;

        if (life <= 0)
            anim.SetTrigger("Dead");
        if (transform.position.y < limitY || ThirdCamera.gameOver)
            GetComponent<DestroyController>().enabled = true;

        rb.velocity = Vector3.down * speed;
        if (player.position.x > transform.position.x && transform.lossyScale.x > 0 || player.position.x < transform.position.x && transform.lossyScale.x < 0)
        {
            float x = -transform.localScale.x;
            transform.localScale = new Vector3(x, transform.localScale.y);
        }
    }

    public void DeadLanguage()
    {
        StartCoroutine(GameController.Instance.Language(transform, "???", "..."));
        StartCoroutine(GameController.Instance.Language(GameController.Instance.player.transform, "^_^", "･◡･"));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Bullet" && collision.gameObject.GetComponent<BulletController>().playerBullet)
            life--;
        if (collision.transform.tag == "Bullet" && collision.gameObject.GetComponent<BossEnemyBullet>().skate)
            life--;
    }

    private void OnDestroy()
    {
        Boss01.summonChildCount--;
    }
}