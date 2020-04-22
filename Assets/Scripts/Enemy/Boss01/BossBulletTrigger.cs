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

    private Tween _bulletTween;
    private Vector3 player;
    private Vector3 spawnPos;
    private Vector3 initialPos;
    private float maXPosX;

    void Awake()
    {
        spawnPos = transform.parent.Find("SpawnPos").position;
        initialPos = spawnPos;
        maXPosX = transform.parent.Find("MaxPos").position.x;
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        spawnPos = initialPos;
        player = GameController.Instance.player.transform.position;
        for (; spawnPos.x < maXPosX;)
        {
            float kArch = Random.Range(0f, 1f);
            Vector3[] pointPos = GetWaveBullet(spawnPos, kArch);
            GameObject child = Instantiate(trackBullet, spawnPos, Quaternion.identity);
            if (player.x < child.transform.position.x && child.transform.lossyScale.x > 0)
            {
                float x = -child.transform.localScale.x;
                child.transform.localScale =
                    new Vector3(x, child.transform.localScale.y);
            }
            _bulletTween = child.transform.DOPath(pointPos, speed, PathType.CatmullRom).OnWaypointChange(p =>
            {
                GameController.Instance.BulletLookAt(child.transform, player);
            });
            yield return spawnPos += Vector3.right;
        }
        yield return 1;
        yield return Boss01.isSkill = false;
        this.enabled = false;
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
