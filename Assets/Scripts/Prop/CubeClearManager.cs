using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeClearManager : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        foreach (Transform item in transform)
            if (!item.GetComponent<CubeClear>().isStay)
                return;

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform col = transform.GetChild(i).GetComponent<CubeClear>().colSprite;
            for (int j = 0; j < col.GetSiblingIndex(); j++)
            {
                Vector3 newFK = col.parent.GetChild(j).position;
                newFK.y -= col.GetComponent<BoxCollider2D>().bounds.size.y;
                col.parent.GetChild(j).position = newFK;
            }
            col.gameObject.SetActive(false);
            if (col.parent.GetComponentInParent<CubeManager>())
                col.parent.GetComponentInParent<CubeManager>().isOver = true;
        }

        gameObject.SetActive(false);
    }

}
