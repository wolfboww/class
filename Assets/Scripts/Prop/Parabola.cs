using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Parabola : MonoBehaviour
{
    public GameObject bullet;

    private float speed = 2;
    private float timer = 0;
    private Transform target;
    private Tween _bulletTween;
    private TweenParams para;
    public float dis =1;

    // Start is called before the first frame update
    void Start()
    {
        TweenParams para = new TweenParams();
        para.SetLoops(-1, LoopType.Restart);
        target = transform.parent;
    }

    void Update()
    {
        if (timer >dis)
        {
            StartCoroutine(WaterCurtain());
            timer = 0;
        }
        else
            timer += Time.deltaTime;
    }

    IEnumerator WaterCurtain()
    {
        GameObject child = Instantiate(bullet, transform.position, Quaternion.identity);
        _bulletTween = child.transform.DOPath(GetWaveBullet(), speed, PathType.CatmullRom).OnWaypointChange(p =>
        {
            GameController.Instance.BulletLookAt(child.transform, target.position);
        }).OnComplete(() => Destroy(child));
        yield return 1;
    }

    public Vector3[] GetWaveBullet()
    {
        Vector3 distance = transform.position - target.position;
        Vector3[] points = new Vector3[2];
        float radius = distance.magnitude * Mathf.Sqrt(0.5f * (2 - 0.5f)) / 4;
        Vector3 normalVec = Vector3.Cross(Vector3.forward, distance).normalized;
        if (target.position.x > transform.position.x)
            points[0] = (target.position + transform.position) * 0.5f - 0.5f * radius * normalVec;
        else
            points[0] = (target.position + transform.position) * 0.5f + 0.5f * radius * normalVec;
        points[1] = target.position;
        return points;
    }
}
