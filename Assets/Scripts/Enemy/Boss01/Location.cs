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
    private Transform linePoint;

    private float limitX;
    private Vector3 pos;
    private int locateLife;

    private Animator anim;
    private TweenParams para = new TweenParams();

    // Start is called before the first frame update
    void Start()
    {
        boss = transform.parent.parent.Find("Boss");
        player = GameController.Instance.player.transform;
        dir = transform.parent.Find("Dir");
        linePoint = transform.Find("LinePoint");

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
        dir.position = Vector3.Distance(RayHit(), Vector3.zero) < 1f ?
            new Vector3(pos.x + Mathf.RoundToInt(disX) * limitX, player.GetComponent<BoxCollider2D>().bounds.min.y) : RayHit();

        if (GameController.isBoss)
            DrawCurve(linePoint.position, boss.Find("LocationPoint").GetChild(0).position, boss.Find("LocationPoint").position, linePoint.GetComponent<LineRenderer>());

        if (life <= 0 || ThirdCamera.gameOver)
            anim.SetTrigger("Dead");
    }

    private Vector3 RayHit()
    {
        RaycastHit2D hit = Physics2D.Raycast(boss.Find("WeaponPoint").position, (player.GetComponent<BoxCollider2D>().bounds.min - boss.Find("WeaponPoint").position).normalized, 10, 1 << 8);

        return hit.point;
    }

    private void DrawCurve(Vector3 point1, Vector3 point2, Vector3 point3, LineRenderer MyL)
    {

        int vertexCount = 30;//采样点数量
        List<Vector3> pointList = new List<Vector3>();

        for (float ratio = 0; ratio <= 1; ratio += 1.0f / vertexCount)
        {
            Vector3 tangentLineVertex1 = Vector3.Lerp(point1, point2, ratio);
            Vector3 tangentLineVectex2 = Vector3.Lerp(point2, point3, ratio);
            Vector3 bezierPoint = Vector3.Lerp(tangentLineVertex1, tangentLineVectex2, ratio);
            pointList.Add(bezierPoint);
        }
        MyL.positionCount = pointList.Count;
        MyL.SetPositions(pointList.ToArray());
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
