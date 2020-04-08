using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Pendulum : MonoBehaviour
{
    public Transform m_anchor;      //圆点
    public float g = 9.8f;          //重力加速度

    private Vector3 m_rotateAxis;   //旋转轴
    private float w = 0;            //角速度
    // Use this for initialization
    void Start()
    {
        m_rotateAxis = Vector3.Cross(transform.position - m_anchor.position, Vector3.down);
    }

    // Update is called once per frame
    void Update()
    {
        DoPhysics();
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
}