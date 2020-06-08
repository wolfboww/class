using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Camera3D : MonoBehaviour
{
    public ReviveInstant reviveInstant;
    private Transform cube;
    private int index = 0;
    private bool action = false;
    private float desCubeY = 2.5f;
    private float timer = 0;

    void Update()
    {
        if (GameController.isRevive)
        {
            action = false;
            desCubeY = 2.5f;
            timer = 0;
        }

        if (!GetComponent<Rigidbody>())
            return;

        cube = reviveInstant.oldPrefab.transform;
        for (int i = 0; i < index; i++)
        {
            for (int j = 0; j < cube.GetChild(i).childCount; j++)
            {
                SetMaterialsColor(cube.GetChild(i).GetChild(j).GetComponent<Renderer>(), 0.5f);
            }
        }

        for (int i = index; i < cube.childCount - 2; i++)
        {
            for (int j = 0; j < cube.GetChild(i).childCount; j++)
            {
                SetMaterialsColor(cube.GetChild(i).GetChild(j).GetComponent<Renderer>(), 1f);
            }
        }

        if (action)
        {
            cube.Find("Trigger").Find("Dead").gameObject.SetActive(true);
            for (int i = 0; i < cube.childCount - 2; i++)
            {
                foreach (Transform item in cube.GetChild(i))
                {
                    if (item.localPosition.y < desCubeY)
                    {
                        GameObject child = Instantiate(ColliNameManager.Instance.CubeAnim, item.position, Quaternion.identity);
                        Destroy(child, 2);
                        Destroy(item.gameObject);
                    }
                }
            }

            timer += Time.deltaTime;
            if (timer >= 5)
            {
                desCubeY += 1;
                timer = 0;
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

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.parent.name != "Trigger")
            return;
        if (other.tag != "Plane")
            return;

        if (int.Parse(other.name) < 3)
        {
            if (other.GetComponent<BoxCollider>().bounds.max.z < transform.position.z)
                index = int.Parse(other.name);
            else if (other.GetComponent<BoxCollider>().bounds.min.z > transform.position.z)
                index = int.Parse(other.name) - 1;
        }
        else
            action = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Injurant")
        {
            if (GetComponent<CollisionController>().return2d != null)
                return;
            GetComponent<CollisionController>().return2d = StartCoroutine(GetComponent<CollisionController>().Return2D());
        }

        if (other.name == "Leave")
        {
            GameController.Instance.ActiveCam().gameObject.SetActive(false);
            ColliNameManager.Instance.MainCamera.gameObject.SetActive(true);
            ColliNameManager.Instance.MainCamera.GetComponent<MusicController>().enabled = true;
            GetComponent<MoveController>().virtual3D = false;
        }
    }
}