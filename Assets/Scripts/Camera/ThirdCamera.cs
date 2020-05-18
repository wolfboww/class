using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ThirdCamera : MonoBehaviour
{
    public GameObject TVLight;
    public GameObject Boss;
    public GameObject Jxb;
    public Transform JxbLimit;
    public Transform playerPos;
    public Transform device;
    public static bool gameOver;
    public int bossLife = 100;
    public float maskCD;

    private Transform thirdCamera;
    private Transform forthCamera;
    private Transform bossPos;
    private GameObject player;
    private Animator anim;
    private Coroutine async;

    private float timer = 0;

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
        {
            if (async != null)
                return;
            async = StartCoroutine(GameReturn());
        }

        if (!Jxb.activeInHierarchy && GameController.isBoss)
        {
            if (timer >= maskCD)
            {
                timer = 0;
                Jxb.transform.position = new Vector3(Random.Range(JxbLimit.GetChild(0).position.x, JxbLimit.GetChild(1).position.x), Jxb.transform.position.y);
                Jxb.SetActive(true);
            }
            else
                timer += Time.deltaTime;
        }
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
        yield return 1;
        forthCamera.GetComponent<CameraBlack>().enabled = true;
        yield return new WaitUntil(() => forthCamera.GetComponent<BrightnessSaturationAndContrast>().brightness >= 0.9f);
        Boss.GetComponent<Animator>().SetTrigger("Run");
        yield return new WaitUntil(() => Boss.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);
        Boss01.life = bossLife;
        Boss.GetComponent<Animator>().SetTrigger("Action");
        GetComponent<StopControl>().PlayerStop(0);
        bossPos.GetComponent<BoxCollider2D>().isTrigger = false;
        async = null;
    }

    IEnumerator GameReturn()
    {
        yield return 1;
        gameOver = false;
        Boss01.isSkill = false;
        Boss.GetComponent<Animator>().SetTrigger("Win");
        bossPos.GetComponent<BoxCollider2D>().isTrigger = true;
        yield return new WaitUntil(() => Boss.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);
        ColliNameManager.Instance.BossSkate.GetComponent<BossSkate>().childReady = false;
        forthCamera.GetComponent<CameraBlack>().enabled = false;
        yield return forthCamera.GetComponent<CameraBlack>().targetBrightness = 0;
        forthCamera.GetComponent<BrightnessSaturationAndContrast>().enabled = true;
        yield return forthCamera.GetComponent<CameraBlack>().enabled = true;
        yield return new WaitUntil(() => forthCamera.GetComponent<BrightnessSaturationAndContrast>().brightness <= 0.1f);
        forthCamera.GetComponent<CameraBlack>().enabled = false;
        thirdCamera.GetComponent<BrightnessSaturationAndContrast>().brightness = 1;
        ColliNameManager.Instance.MainCamera.gameObject.SetActive(true);
        forthCamera.gameObject.SetActive(false);
        Boss.SetActive(false);
        GameController.isBoss = false;
        yield return Boss.transform.position = bossPos.position;
        if (transform.position.x > player.transform.position.x && transform.lossyScale.x < 0 || transform.position.x < player.transform.position.x && transform.lossyScale.x > 0)
            Boss.GetComponent<Animator>().SetTrigger("Return");

        if (ColliNameManager.Instance.BossSkate.transform.parent != Boss.transform.parent.Find("SkatePos"))
            ColliNameManager.Instance.BossSkate.transform.SetParent(Boss.transform.parent.Find("SkatePos"));
        Boss.transform.parent.Find("SkatePos").localPosition = Vector3.zero;
        ColliNameManager.Instance.BossSkate.GetComponent<Rigidbody2D>().mass = 100;
        yield return forthCamera.GetComponent<CameraBlack>().targetBrightness = 1;
        yield return new WaitForSeconds(2);
        //yield return StartCoroutine(GameStart());
        //yield return 1;
        //yield return Boss.GetComponent<Boss01>().enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            StartCoroutine(BossAnim());
    }
}
