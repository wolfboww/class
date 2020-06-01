using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeClear : MonoBehaviour
{
    [HideInInspector]
    public bool isStay;
    [HideInInspector]
    public Transform colSprite;

    private void Update()
    {
        if (GameController.isRevive)
            isStay = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        colSprite = collision.gameObject.transform;
        isStay = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        colSprite = null;
        isStay = false;
    }
}
