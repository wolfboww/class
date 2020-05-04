using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PendulunAnim : MonoBehaviour
{
    public GameObject ballObj;

    private Animator anim;
    private Transform ball;
    private Transform player;

    private float timer = 1;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        ball = transform.GetChild(0).Find("ball");
        player = GameController.Instance.player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.isRevive)
        {
            ballObj.transform.SetParent(transform.GetChild(0));
            ballObj.transform.localPosition = Vector3.zero;
            anim.SetFloat("Distance", 0);
            timer = 1;
        }

        if (GetComponentInChildren<PolygonCollider2D>().bounds.min.y > player.position.y)
            anim.SetFloat("Distance", 1);

        if (!anim.GetFloat("Distance").Equals(0))
        {
            if (timer > 6)
                return;
            timer += Time.deltaTime;
            anim.SetFloat("Distance", timer);
        }

    }

    private void FrameAttack()
    {
        ball.transform.SetParent(null);
    }
}
