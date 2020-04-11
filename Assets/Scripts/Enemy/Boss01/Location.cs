using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Location : MonoBehaviour
{
    private Transform boss;
    private Transform player;

    private Animator anim;
    private TweenParams para = new TweenParams();

    // Start is called before the first frame update
    void Start()
    {
        boss = transform.root;
        player = GameController.Instance.player.transform;

        anim = GetComponent<Animator>();
        para.SetLoops(-1, LoopType.Yoyo);
        transform.DOLocalMoveY(1, 2).SetAs(para);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.parent.lossyScale.x < 0)
        {
            float x = -transform.parent.localScale.x;
            transform.parent.localScale = new Vector3(x, transform.parent.localScale.y);
        }

        float distance = (player.position.x - boss.position.x) / 5;
        anim.SetFloat("Angle", distance);
    }
}
