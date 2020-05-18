using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BossJxb : MonoBehaviour
{
    public GameObject[] mask;

    private Transform maskPos;
    private Transform target;
    private Vector3 targetPos;
    private float colMin;
    private Animator anim;

    private GameObject child;
    private Tween _tween;
    private float timer = 0;
    private bool maskScale = true;

    // Start is called before the first frame update
    void Awake()
    {
        maskPos = transform.Find("MaskPos");
        target = transform.Find("Target");
        targetPos = target.localPosition;
        anim = GetComponent<Animator>();
        colMin = GetComponent<BoxCollider2D>().bounds.min.y - GetComponent<BoxCollider2D>().bounds.center.y;
    }

    private void OnEnable()
    {
        maskScale = true;
        child = Instantiate(mask[Random.Range(0, 2)], maskPos.position, Quaternion.identity);
        child.transform.SetParent(transform.root.Find("Mask"));
        child.transform.localScale = Vector3.one;
        target.localPosition = targetPos - new Vector3(0, Random.Range(-1.0f, 0), 0);
        StartCoroutine(Jxb());
    }

    // Update is called once per frame
    void Update()
    {
        if (maskScale)
            child.transform.position = maskPos.position;
    }

    IEnumerator Jxb()
    {
        anim.SetTrigger("Get");
        float scaleY = (target.position.y - transform.position.y) / colMin;
        _tween = transform.DOScaleY(scaleY, 2);
        yield return _tween.WaitForCompletion();
        anim.SetTrigger("Turn");
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("JXBIdle"));
        yield return maskScale = false;
        transform.localScale = Vector3.one;
        target.localPosition = targetPos;
        gameObject.SetActive(false);
    }
}
