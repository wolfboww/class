﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ThirdCamera : MonoBehaviour
{
    public GameObject TVLight;
    public GameObject Boss;
    public Transform playerPos;
    public Transform device;
    public static bool gameOver;

    private Transform thirdCamera;
    private Transform forthCamera;
    private Transform bossPos;
    private GameObject player;
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        thirdCamera = transform.Find("ThirdCamera");
        forthCamera = transform.Find("ForthCamera");
        bossPos = transform.Find("BossPos");
        player = GameController.Instance.player;
        anim = player.GetComponent<Animator>();
        gameOver = false;
    }

    void Update()
    {
        if (gameOver)
            StartCoroutine(GameReturn());
    }

    IEnumerator BossAnim()
    {
        GameController.isBoss = true;
        ColliNameManager.Instance.MainCamera.gameObject.SetActive(false);
        transform.Find("ThirdCamera").gameObject.SetActive(true);
        yield return 0;
        GetComponent<StopControl>().PlayerStop(1);
        yield return 1;
        player.transform.position = transform.Find("PlayerPos").position;
        player.transform.localScale = new Vector3(-1, 1, 1);
        anim.SetFloat("Shoot", 1);
        player.GetComponent<AnimatorController>().bullet = ColliNameManager.Instance.IfBullet;
        player.GetComponent<AnimatorController>().Shoot();
        TVLight.GetComponent<Animator>().SetTrigger("Do");
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1f);
        thirdCamera.GetComponent<CameraBlack>().enabled = true;
        yield return new WaitUntil(() => thirdCamera.GetComponent<BrightnessSaturationAndContrast>().brightness <= 0.1f);
        thirdCamera.gameObject.SetActive(false);
        forthCamera.gameObject.SetActive(true);
        yield return new WaitUntil(() => player.GetComponentInChildren<SpriteRenderer>().enabled);
        anim.SetFloat("Shoot", 0);
        yield return StartCoroutine(GameStart());
    }

    IEnumerator GameStart()
    {
        Boss.SetActive(true);
        player.transform.position = playerPos.position;
        player.transform.localScale = new Vector3(1, 1, 1);
        forthCamera.GetComponent<CameraBlack>().enabled = true;
        yield return new WaitUntil(() => forthCamera.GetComponent<BrightnessSaturationAndContrast>().brightness >= 0.9f);
        Boss.GetComponent<Animator>().SetTrigger("Run");
        GetComponent<StopControl>().PlayerStop(0);
    }

    IEnumerator GameReturn()
    {
        yield return 1;
        gameOver = false;
        Boss01.isSkill = false;
        ColliNameManager.Instance.BossSkate.GetComponent<BossSkate>().childReady = false;
        forthCamera.GetComponent<CameraBlack>().enabled = false;
        yield return forthCamera.GetComponent<CameraBlack>().targetBrightness = 0;
        forthCamera.GetComponent<BrightnessSaturationAndContrast>().enabled = true;
        yield return forthCamera.GetComponent<CameraBlack>().enabled = true;
        yield return new WaitUntil(() => forthCamera.GetComponent<BrightnessSaturationAndContrast>().brightness <= 0.1f);
        forthCamera.GetComponent<CameraBlack>().enabled = false;
        yield return Boss.transform.position = bossPos.position;
        if (transform.position.x > player.transform.position.x && transform.lossyScale.x < 0 || transform.position.x < player.transform.position.x && transform.lossyScale.x > 0)
            Boss.GetComponent<Animator>().SetTrigger("Return");

        if (ColliNameManager.Instance.BossSkate.transform.parent != Boss.transform.parent.Find("SkatePos"))
            ColliNameManager.Instance.BossSkate.transform.SetParent(Boss.transform.parent.Find("SkatePos"));
        yield return forthCamera.GetComponent<CameraBlack>().targetBrightness = 1;
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(GameStart());
        yield return StartCoroutine(Boss.GetComponent<Boss01>().BossControl());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            StartCoroutine(BossAnim());
    }
}
