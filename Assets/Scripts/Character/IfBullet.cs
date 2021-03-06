﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IfBullet : MonoBehaviour
{
    static public bool bemask = false;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Contains("Mask"))
        {
            if (!collision.transform.Find("hatPos"))
                return;

            bemask = true;
            GameController.Instance.Mask(collision.gameObject);
        }
    }
}
