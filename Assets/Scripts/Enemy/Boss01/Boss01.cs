using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Boss01 : MonoBehaviour
{
    public enum State
    {
        Idle, Run, Attack, Skill1, Skill2, Skill3, Skill4, Skill5
    }
    public State state = State.Idle;
    public static bool isSkill;
    public Transform[] TVButton;
    public Transform bossUI;
    public GameObject device;
    public GameObject bossSummon;
    public static int summonChildCount;
    public GameObject bossBullet;
    public GameObject bulletTrigger;
    public GameObject bulletEffect;
    public float moveSpeed;
    public static int life;
    [HideInInspector]
    public int skill;
    public static bool isTrigger;

    private Rigidbody2D rig;
    private Animator anim;

    private GameObject bossSkate;
    private Transform player;
    private Transform groundCheck;
    private Transform location;
    private Vector3[] limit;
    private Transform skate;
    private Transform weaponPoint;
    private Vector3 weaponPos;
    private Transform trigger;
    private GameObject bulletClone;
    private Animator deAnim;
    private Transform boundary;

    private Vector3 skatePos;
    private bool onPlane;
    private bool complete = false;
    private int summonTime = 10;
    private float checkRadius = 0.5f;
    private float jumpForce = 10;
    private float rotateSpeed = 5;
    private float angle;

    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rig = GetComponent<Rigidbody2D>();

        groundCheck = transform.Find("GroundCheck");
        location = transform.parent.Find("Location");
        skate = transform.parent.Find("SkatePos");
        skatePos = skate.localPosition;
        bossSkate = ColliNameManager.Instance.BossSkate;
        limit = new Vector3[location.childCount];
        limit[0] = location.Find("Left").position;
        limit[1] = location.Find("Right").position;
        player = GameController.Instance.player.transform;
        weaponPoint = transform.Find("WeaponPoint");
        weaponPos = weaponPoint.position;

        boundary = transform.parent.Find("Boundary").GetChild(0);
        trigger = bulletTrigger.transform.GetChild(0);
        deAnim = device.GetComponent<Animator>();
        summonChildCount = summonTime;
        life = 150;
        isTrigger = false;
        isSkill = false;
    }

    void OnEnable()
    {
        StartCoroutine(BossControl());
    }

    public IEnumerator BossControl()
    {
        yield return state = State.Idle;
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(SkillAndShow(State.Skill1));
        yield return StartCoroutine(SkillAndShow(State.Skill3));
        yield return StartCoroutine(SkillAndShow(State.Skill2));
        yield return StartCoroutine(SkillAndShow(State.Skill5));
        yield return StartCoroutine(SkillAndShow(State.Skill4));
    }

    IEnumerator SkillAndShow(State s)
    {
        yield return state = s;
        yield return new WaitUntil(() => !isSkill);
        yield return StartCoroutine(TVLight(0, 0));
        anim.SetTrigger("Show");
        yield return new WaitWhile(() => anim.GetCurrentAnimatorStateInfo(0).IsName("BossShow"));
        yield return new WaitForSeconds(2);
    }

    void FixedUpdate()
    {
        StartCoroutine(Back(boundary));
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("BossShoot") || anim.GetCurrentAnimatorStateInfo(0).IsName("BossRunShoot"))
        {
            transform.position = Vector2.MoveTowards(transform.position, transform.position + (transform.lossyScale.x > 0 ? Vector3.left : Vector3.right) * moveSpeed * 0.01f, 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        onPlane = Physics2D.OverlapCircle(groundCheck.position, checkRadius, 1 << LayerMask.NameToLayer("Plane"));
        anim.SetBool("OnPlane", onPlane);

        if (ThirdCamera.gameOver)
        {
            StopAllCoroutines();
            return;
        }

        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("BossJump"))
        {
            Vector3 pos = skatePos;
            if (skate.lossyScale.x > 0)
                pos.x += transform.localPosition.x;
            else
                pos.x = transform.localPosition.x - skatePos.x;
            skate.localPosition = pos;
        }

        if (bossSkate.transform.parent == skate)
            bossSkate.transform.localPosition = Vector3.zero;

        if (state != State.Skill3)
        {
            if (Mathf.Abs(player.position.y - weaponPos.y) < 2.5f)
                angle = 0.5f;
            else
                angle = weaponPoint.position.y > player.position.y ? 1 : 0;
            anim.SetFloat("Angle", angle);
        }

        if (bulletTrigger.activeInHierarchy)
            bulletTrigger.transform.position = transform.Find("TriggerPos").position;


        if (!isSkill || ThirdCamera.gameOver)
        {
            if (bulletTrigger.activeInHierarchy)
                bulletTrigger.SetActive(false);
            if (location.Find("LocatePos").gameObject.activeInHierarchy)
                location.Find("LocatePos").gameObject.SetActive(false);
            if (device.GetComponent<SpriteRenderer>().enabled)
                device.GetComponent<SpriteRenderer>().enabled = false;
        }

        if (life > 100)
        {
            bossUI.GetChild(0).GetChild(0).GetComponent<Image>().DOFillAmount((float)(life - 100) / 50, 0.1f);
            bossUI.GetChild(1).GetChild(0).GetComponent<Image>().fillAmount = 1;
            bossUI.GetChild(2).GetChild(0).GetComponent<Image>().fillAmount = 1;
        }
        else if (life > 50)
        {
            bossUI.GetChild(0).GetChild(0).GetComponent<Image>().fillAmount = 0;
            bossUI.GetChild(1).GetChild(0).GetComponent<Image>().DOFillAmount((float)(life - 50) / 50, 0.1f);
            bossUI.GetChild(2).GetChild(0).GetComponent<Image>().fillAmount = 1;
        }
        else if (life > 0)
        {
            bossUI.GetChild(0).GetChild(0).GetComponent<Image>().fillAmount = 0;
            bossUI.GetChild(1).GetChild(0).GetComponent<Image>().fillAmount = 0;
            bossUI.GetChild(2).GetChild(0).GetComponent<Image>().DOFillAmount((float)life / 50, 0.1f);
        }
        else
            StartCoroutine(Anim("Dead"));

        StateController();
    }

    private void StateController()
    {
        if (!complete)
        {
            if (state != State.Idle)
                complete = true;

            switch (state)
            {
                case State.Run:
                    StartCoroutine(Anim("Run"));
                    break;
                case State.Attack:
                    skill = 0;
                    Shoot();
                    break;
                case State.Skill1:  // 定位
                    StartCoroutine(TVLight(0, 90));
                    skill:
                    skill = 1;
                    StartCoroutine(Locate());
                    break;
                case State.Skill2:  // 召唤伞兵
                    StartCoroutine(TVLight(90, 90));
                    StartCoroutine(Jump());
                    break;
                case State.Skill3:  // 子弹雨
                    skill = 3;
                    StartCoroutine(TVLight(90, 0));
                    StartCoroutine(BulletRain());
                    break;
                case State.Skill4:
                case State.Skill5:
                    goto skill;
                case State.Idle:
                    bulletClone = null;
                    isTrigger = false;
                    break;
            }
        }
        else if (state == State.Idle)
            complete = false;
    }

    IEnumerator TVLight(float TV1, float TV2)
    {
        TVButton[0].GetComponent<AudioSource>().enabled = true;

        while (Mathf.Abs(TVButton[0].eulerAngles.z - TV1) > 0.1f)
        {
            if (TVButton[0].eulerAngles.z < TV1)
                TVButton[0].Rotate(Vector3.forward, rotateSpeed);
            else
                TVButton[0].Rotate(Vector3.back, rotateSpeed);
            yield return 1;
        }

        while (Mathf.Abs(TVButton[1].eulerAngles.z - TV2) > 0.1f)
        {
            if (TVButton[1].eulerAngles.z < TV2)
                TVButton[1].Rotate(Vector3.forward, rotateSpeed);
            else
                TVButton[1].Rotate(Vector3.back, rotateSpeed);
            yield return 1;
        }
        yield return new WaitForSeconds(1);
        TVButton[0].GetComponent<AudioSource>().enabled = false;
    }


    IEnumerator Anim(string name)
    {
        yield return 1;
        anim.SetTrigger(name);
        yield return 1;
        anim.ResetTrigger(name);
    }

    IEnumerator Locate()
    {
        isSkill = true;
        Vector3 pos = location.position;
        if (player.position.x < limit[0].x)
            pos.x = limit[0].x;
        else if (player.position.x > limit[1].x)
            pos.x = limit[1].x;
        else
            pos.x = player.position.x;

        yield return StartCoroutine(Back(player));
        yield return location.Find("LocatePos").position = pos;
        location.Find("LocatePos").gameObject.SetActive(true);

        yield return new WaitUntil(() => location.Find("LocatePos").gameObject.activeInHierarchy);
        while (true)
        {

            InvokeRepeating("Shoot", 1, 2);     // 散弹发射间隔
            if (state == State.Skill4)
            {
                skill = 2;
                StartCoroutine(Jump());
                yield return new WaitUntil(() => summonChildCount == summonTime);
                yield return new WaitUntil(() => summonChildCount <= 0);
            }
            else if (state == State.Skill5)
            {
                StartCoroutine(BulletRain());
                yield return new WaitUntil(() => trigger.GetComponent<BossBulletTrigger>().enabled);
                yield return new WaitWhile(() => trigger.GetComponent<BossBulletTrigger>().enabled);
                bulletTrigger.SetActive(false);
            }
            else break;

            if (!location.Find("LocatePos").gameObject.activeInHierarchy)
                break;
            yield return new WaitForSeconds(5);
        }
        state = State.Idle;

        yield return new WaitWhile(() => location.Find("LocatePos").gameObject.activeInHierarchy);
        isSkill = false;
        StopCoroutine(Locate());
    }

    private void Shoot()
    {
        switch (skill)
        {
            case 0:
                if (transform.position.x > player.position.x && transform.lossyScale.x < 0 || transform.position.x < player.position.x && transform.lossyScale.x > 0)
                    return;
                bulletClone = Instantiate(
                bossBullet, weaponPoint.position, Quaternion.identity);
                bulletClone.GetComponent<BossEnemyBullet>().dir = player.position;
                GameController.Instance.BulletLookAt(bulletClone.transform, player.position);
                break;
            case 1:

                if (!location.Find("LocatePos").gameObject.activeInHierarchy)
                    CancelInvoke();
                if (state == State.Skill2 || state == State.Skill3 || ThirdCamera.gameOver)
                    CancelInvoke();
                if (transform.position.x > player.position.x && transform.lossyScale.x < 0 || transform.position.x < player.position.x && transform.lossyScale.x > 0)
                    return;

                bulletClone = Instantiate(
                    bossBullet, weaponPoint.position, Quaternion.identity);
                bulletClone.GetComponent<BossEnemyBullet>().dir = location.Find("Dir").position;
                GameController.Instance.BulletLookAt(bulletClone.transform, location.Find("Dir").position);
                break;
            case 2:
                break;
            case 3:
                bulletClone = Instantiate(
                    bossBullet, weaponPoint.position, Quaternion.identity);
                bulletClone.GetComponent<BossEnemyBullet>().dir = (weaponPoint.position - trigger.position).normalized * 20;
                GameController.Instance.BulletLookAt(bulletClone.transform, (weaponPoint.position - trigger.position).normalized);
                break;
        }
    }

    IEnumerator Jump()
    {
        isSkill = true;
        deAnim.ResetTrigger("Return");
        device.GetComponent<SpriteRenderer>().enabled = true;
        deAnim.SetInteger("Step", 1);
        yield return StartCoroutine(Back(device.transform));
        yield return 0;
        do
        {
            yield return StartCoroutine(Anim("Run"));
        }
        while (Mathf.Abs(transform.position.x - device.transform.position.x) > moveSpeed);
        yield return new WaitWhile(() => anim.GetCurrentAnimatorStateInfo(0).IsName("BossReturn"));
        yield return StartCoroutine(Anim("Jump"));
        yield return new WaitUntil(
            () => anim.GetCurrentAnimatorStateInfo(0).IsName("BossJump"));
        skate.DetachChildren();
        BossSkate.canGet = true;
        yield return rig.velocity = new Vector2(rig.velocity.x, jumpForce);
        if (state != State.Skill4)
            state = State.Idle;
        yield return new WaitForSeconds(1);
        yield return new WaitUntil(() => onPlane);
        yield return new WaitWhile(() => BossSkate.isCol);
        bossSkate.transform.SetParent(skate);
        BossSkate.canGet = false;
        yield return 0;
        if (transform.position.x > player.position.x && transform.lossyScale.x < 0 || transform.position.x < player.position.x && transform.lossyScale.x > 0)
            yield return StartCoroutine(Anim("Return"));
        StopCoroutine(Jump());
    }

    IEnumerator Summon()
    {
        skill = 1;
        summonChildCount = summonTime;
        for (int i = 0; i < summonTime; i++)
        {
            if (state == State.Skill1 || state == State.Skill3)
                break;

            Vector3 pos = device.transform.parent.Find("EnemySpawn").position;
            yield return pos.x = Random.Range(limit[0].x, limit[1].x);
            GameObject summon = Instantiate(bossSummon, device.transform.parent.Find("EnemySpawn"));
            summon.transform.position = pos;
            summon.GetComponent<BossEnemy>().limitY =
                device.transform.parent.Find("LimitY").position.y;
            yield return new WaitForSeconds(1);
        }
        yield return bossSkate.GetComponent<BossSkate>().childReady = true;
        yield return new WaitUntil(() => summonChildCount <= 0);
        if (state != State.Skill4)
            isSkill = false;
        deAnim.SetTrigger("Return");
    }

    IEnumerator BulletRain()
    {
        isSkill = true;
        yield return StartCoroutine(Back(player));
        yield return new WaitWhile(
            () => anim.GetCurrentAnimatorStateInfo(0).IsName("BossReturn"));
        bulletTrigger.SetActive(true);
        yield return new WaitForSeconds(1);
        anim.SetFloat("Angle", 0);
        yield return 1;
        skill = 3;
        Shoot();
        yield return new WaitUntil(() => isTrigger);
        if (state == State.Skill5)
            skill = 1;
        else
            state = State.Idle;
        yield return new WaitForSeconds(1);
        trigger.GetComponent<BossBulletTrigger>().enabled = true;
        anim.SetFloat("Angle", 0.5f);
        yield return 1;

        StopCoroutine(BulletRain());
    }

    IEnumerator Back(Transform target)
    {
        if (transform.position.x > target.position.x && transform.lossyScale.x < 0 || transform.position.x < target.position.x && transform.lossyScale.x > 0)
        {
            yield return StartCoroutine(Anim("Return"));
            yield return new WaitUntil(() => ColliNameManager.Instance.BossSkate.GetComponentInChildren<SpriteRenderer>().enabled);
        }
    }

    private void Return()
    {
        float x = -transform.localScale.x;
        transform.localScale = new Vector3(x, transform.localScale.y);
        skate.localScale = new Vector3(x, skate.localScale.y);

        boundary = boundary.name.Equals("Min") ? transform.parent.Find("Boundary").GetChild(1) : transform.parent.Find("Boundary").GetChild(0);
    }

    private void HideSkate(int i)
    {
        ColliNameManager.Instance.BossSkate.GetComponentInChildren<SpriteRenderer>().enabled =
            i.Equals(0) ? true : false;
    }

    private void Dead()
    {
        GameController.isBoss = false;

    }

    private void Win()
    {
        GameObject child = Instantiate(ColliNameManager.Instance.BossWinner, transform.position + new Vector3(0, 5), Quaternion.identity);
        child.transform.localScale *= 1.5f;
        Destroy(child, child.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length * 2);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == device.gameObject)
        {
            deAnim.SetInteger("Step", 2);
            StartCoroutine(Summon());
        }
        if (collision.gameObject.GetComponent<BulletController>())
            if (collision.gameObject.GetComponent<BulletController>().playerBullet)
            {
                GameObject effect = Instantiate(bulletEffect, collision.contacts[0].point, Quaternion.identity);
                if (collision.contacts[0].point.y < (GetComponent<PolygonCollider2D>().bounds.max.y + GetComponent<PolygonCollider2D>().bounds.min.y * 2) / 3)
                    life -= 4;
                else if (collision.contacts[0].point.y > (GetComponent<PolygonCollider2D>().bounds.min.y + GetComponent<PolygonCollider2D>().bounds.max.y * 2) / 3)
                    life -= 10;
                else
                    life -= 7;
                StartCoroutine(Back(player));
            }
    }
}
