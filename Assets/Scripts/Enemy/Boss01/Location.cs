using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Location : MonoBehaviour
{
    public int life;

    private Transform boss;
    private Transform player;
    private Transform dir;

    private float limitX;
    private Vector3 pos;
    private int locateLife;

    private Animator anim;
    private TweenParams para = new TweenParams();

    // Start is called before the first frame update
    void Start()
    {
        boss = transform.root;
        player = GameController.Instance.player.transform;
        dir = transform.parent.Find("Dir");
        limitX = (transform.parent.Find("Right").transform.position.x - transform.parent.Find("Left").transform.position.x) / 7;
        pos = dir.transform.position;
        locateLife = life;

        anim = GetComponent<Animator>();
        para.SetLoops(-1, LoopType.Yoyo);
    }

    // Update is called once per frame
    void Update()
    {
        float angle = (player.position.x - transform.position.x) / 5;
        anim.SetFloat("Angle", angle);

        float distance = (player.position.x - pos.x) / 5;
        float disX = distance > 0 ? Mathf.RoundToInt(distance) : Mathf.FloorToInt(distance);
        dir.position = new Vector3(pos.x + Mathf.RoundToInt(disX) * limitX, pos.y);

        if (life <= 0 || ThirdCamera.gameOver)
            anim.SetTrigger("Dead");
    }

    private void Float()
    {
        transform.DOLocalMoveY(1, 2).SetAs(para);
    }

    private void DestroyObj()
    {
        gameObject.SetActive(false);
        life = locateLife;
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
