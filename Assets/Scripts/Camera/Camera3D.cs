using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Camera3D : MonoBehaviour
{
    public GameObject Target;
    private List<Renderer> colliderObject;

    void Start()
    {
        colliderObject = new List<Renderer>();
    }
    void Update()
    {
        if (!Target.GetComponent<Rigidbody>())
            return;

        Debug.DrawLine(Target.transform.position, transform.position, Color.red);
        RaycastHit[] hit = Physics.RaycastAll(Target.transform.position, transform.position);
        if (hit.Length > 0)
        {
            for (int i = 0; i < hit.Length; i++)
            {
                if (!hit[i].collider.gameObject.GetComponent<Renderer>())
                    return;
                Renderer obj = hit[i].collider.gameObject.GetComponent<Renderer>();
                colliderObject.Add(obj);
                SetMaterialsColor(obj, 0.5f);
            }
        }//还原
        else
        {
            for (int i = 0; i < colliderObject.Count; i++)
            {
                Renderer obj = colliderObject[i];
                SetMaterialsColor(obj, 1f);
            }
        }
    }
    /// <summary>
    /// 修改遮挡物体所有材质
    /// </summary>
    /// <param name="_renderer">材质</param>
    /// <param name="Transpa">透明度</param>
    private void SetMaterialsColor(Renderer _renderer, float Transpa)
    {
        //换shader或者修改材质

        //获取当前物体材质球数量
        int materialsNumber = _renderer.sharedMaterials.Length;
        for (int i = 0; i < materialsNumber; i++)
        {

            //获取当前材质球颜色
            Color color = _renderer.materials[i].color;

            //设置透明度  0-1;  0 = 完全透明
            color.a = Transpa;

            //置当前材质球颜色
            _renderer.materials[i].SetColor("_Color", color);
        }
    }
}