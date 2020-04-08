using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PendulunAnim : MonoBehaviour
{
    private Animator anim;
    private Transform ball;
    private Transform player;
    private float timer = 4;

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
        if (GetComponentInChildren<BoxCollider2D>().bounds.min.y > player.position.y)
            anim.SetFloat("Distance", 4);

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
        ball.SetParent(null);
        ball.GetComponent<Rigidbody2D>().gravityScale = 1;
    }

    IEnumerator FrameShake()
    {
        Vector3 frameShake = Random.insideUnitSphere * 0.1f;
        do
        {
            yield return ball.position += frameShake;
            yield return new WaitForSeconds(0.1f);
            yield return ball.position -= frameShake;
        } while (anim.GetCurrentAnimatorStateInfo(0).IsName("FrameShake"));
    }
}
