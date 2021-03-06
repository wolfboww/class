﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BulletUI : MonoBehaviour
{
    public static int bulletNum;
    public static bool light;

    private GameObject IfUI;
    private GameObject ElseUI;
    private GameObject player;

    private Animator anim;
    private float timer = -1;

    // Start is called before the first frame update
    void Start()
    {
        bulletNum = 0;

        IfUI = transform.Find("If").gameObject;
        ElseUI = transform.Find("Else").gameObject;
        player = GameController.Instance.player;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (bulletNum <= 0)
            return;
        else
        {
            ElseUI.SetActive(true);
            IfUI.SetActive(bulletNum >= 2 ? true : false);
        }
        anim.SetBool("GetGun", player.GetComponent<Animator>().GetBool("GetGun"));
        anim.SetInteger("Bullet", Bullet());
        transform.Find("LightUI").gameObject.SetActive(light);
    }

    private int Bullet()
    {
        return player.GetComponent<AnimatorController>().bullet == ColliNameManager.Instance.IfBullet ? 1 : 0;
    }

    private void Fade(int i)
    {
        timer = i;
    }
}
