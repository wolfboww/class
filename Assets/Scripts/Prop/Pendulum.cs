using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Pendulum : MonoBehaviour
{
    public Transform m_anchor;      //圆点
    public float g = 9.8f;          //重力加速度

    private Animator anim;
    private Transform ball;
    private Vector3 m_rotateAxis;   //旋转轴
    private float w = 0;            //角速度
    private float timer = 4;
    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        ball = transform.GetChild(0).Find("ball");
        m_rotateAxis = Vector3.Cross(transform.position - m_anchor.position, Vector3.down);
    }

    // Update is called once per frame
    void Update()
    {
        DoPhysics();
        if (!anim.GetFloat("Distance").Equals(0))
        {
            if (timer > 6)
                return;
            timer += Time.deltaTime;
            anim.SetFloat("Distance", timer);
        }
    }

    private void DoPhysics()
    {
        float r = Vector3.Distance(m_anchor.position, transform.position);
        float l = Vector3.Distance(new Vector3(m_anchor.position.x, transform.position.y, m_anchor.position.z), transform.position);
        Vector3 axis = Vector3.Cross(transform.position - m_anchor.position, Vector3.down);

        l = Vector3.Dot(axis, m_rotateAxis) < 0 ? -l : l;
        float cosalpha = l / r;
        float alpha = (cosalpha * g) / r;

        w += alpha * Time.deltaTime;
        float thelta = w * Time.deltaTime * 180.0f / Mathf.PI;
        transform.RotateAround(m_anchor.position, m_rotateAxis, thelta);
        transform.localEulerAngles = Vector3.zero;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            anim.SetFloat("Distance", 4);
    }
}