using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BossBulletTrigger : MonoBehaviour
{
    public GameObject trackBullet;
    public int bulletNumber;
    public float speed;
    public float spawnCD;

    private List<GameObject> bullet;
    private GameObject effect;
    private Transform boss;

    private Vector3 player;
    private Vector3 spawnPos;
    private Vector3 initialPos;
    private float maXPosX;

    void Awake()
    {
        spawnPos = transform.parent.Find("SpawnPos").position;
        initialPos = spawnPos;
        maXPosX = transform.parent.Find("MaxPos").position.x;
        bullet = new List<GameObject>();
        effect = transform.Find("Effect").gameObject;
        boss = transform.parent.parent.Find("Boss");
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        StartCoroutine(Spawn());
        effect.SetActive(true);
    }

    void OnDisable()
    {
        effect.SetActive(false);
    }

    IEnumerator Spawn()
    {
        spawnPos = initialPos;
        for (; spawnPos.x < maXPosX;)
        {
            player = GameController.Instance.player.transform.position;
            float kArch = Random.Range(0f, 1f);
            Vector3[] pointPos = GetWaveBullet(spawnPos, kArch);
            GameObject child = Instantiate(trackBullet, spawnPos, Quaternion.identity);
            bullet.Add(child);
            if (player.x < child.transform.position.x && child.transform.lossyScale.x > 0)
            {
                float x = -child.transform.localScale.x;
                child.transform.localScale =
                    new Vector3(x, child.transform.localScale.y);
            }
            child.transform.DOMove(pointPos[0], 1).OnPlay(() => GameController.Instance.BulletLookAt(child.transform, pointPos[0])).OnComplete(() =>
                {
                    child.transform.DOPath(pointPos, speed, PathType.CatmullRom).OnWaypointChange(p =>
                         GameController.Instance.BulletLookAt(child.transform, player)).OnComplete(() => bullet.Remove(child));
                });
            yield return spawnPos += Vector3.right;
        }
        yield return new WaitUntil(() => isDestroy(bullet));

        if (boss.GetComponent<Boss01>().state != Boss01.State.Skill5)
            Boss01.isSkill = false;
        this.enabled = false;
    }

    private bool isDestroy(List<GameObject> child)
    {
        foreach (var item in child)
            if (item != null)
                return false;

        return true;
    }


    public Vector3[] GetWaveBullet(Vector3 emitPoint, float kArch)
    {
        Vector3 waveMidPoint = (player + emitPoint) * 0.5f;
        Vector3 distance = player - emitPoint;
        Vector3[] points = new Vector3[3];
        float radius = distance.magnitude * Mathf.Sqrt(kArch * (2 - kArch)) / 4;
        Vector3 normalVec = Vector3.Cross(Vector3.forward, distance).normalized;

        points[0] = new Vector3(emitPoint.x, waveMidPoint.y);
        points[1] = (waveMidPoint + player) * 0.5f - kArch * radius * normalVec;
        points[2] = player;
        return points;
    }
}
