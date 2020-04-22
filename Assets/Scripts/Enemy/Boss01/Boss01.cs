using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss01 : MonoBehaviour
{
    public enum State
    {
        Idle, Run, Attack, Skill1, Skill2, Skill3
    }
    public State state = State.Idle;
    public static bool isSkill;
    public Transform[] TVButton;
    public GameObject device;
    public GameObject bossSummon;
    public static int summonChildCount;
    public GameObject bossBullet;
    public GameObject bulletTrigger;
    public float moveSpeed;
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

    private Vector3 skatePos;
    private bool onPlane;
    private bool complete = false;
    private int skill;
    private int summonTime = 10;
    private float checkRadius = 0.5f;
    private float jumpForce = 10;
    private float rotateSpeed = 1;
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

        trigger = bulletTrigger.transform.GetChild(0);
        deAnim = device.GetComponent<Animator>();
        summonChildCount = summonTime;
        isTrigger = false;
        isSkill = false;
    }

    void OnEnable()
    {
        StartCoroutine(BossControl());
    }

    // Update is called once per frame
    void Update()
    {
        onPlane = Physics2D.OverlapCircle(groundCheck.position, checkRadius, 1 << LayerMask.NameToLayer("Plane"));
        anim.SetBool("OnPlane", onPlane);

        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("BossJump"))
        {
            Vector3 pos = skatePos;
            if (skate.lossyScale.x > 0)
                pos.x += transform.localPosition.x;
            else
                pos.x = transform.localPosition.x - skatePos.x;
            skate.localPosition = pos;
        }

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("BossRunShoot"))
        {
            transform.position = Vector2.MoveTowards(transform.position, transform.position + (transform.lossyScale.x > 0 ? Vector3.left : Vector3.right) * moveSpeed, 1);
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

        if (!isSkill || ThirdCamera.gameOver)
        {
            if (bulletTrigger.activeInHierarchy)
                bulletTrigger.SetActive(false);
            if (location.Find("LocatePos").gameObject.activeInHierarchy)
                location.Find("LocatePos").gameObject.SetActive(false);
        }

        if (ThirdCamera.gameOver)
        {
            StopAllCoroutines();
            deAnim.SetTrigger("Return");
        }

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
                    skill = 1;
                    StartCoroutine(Locate());
                    break;
                case State.Skill2:  // 召唤伞兵
                    StartCoroutine(Jump());
                    break;
                case State.Skill3:  // 子弹雨
                    skill = 3;
                    StartCoroutine(BulletRain());
                    break;
                case State.Idle:
                    StartCoroutine(TVLight(0, 0));
                    bulletClone = null;
                    isTrigger = false;
                    break;
            }
        }
        else if (state == State.Idle)
            complete = false;
    }

    public IEnumerator BossControl()
    {
        yield return state = State.Idle;
        yield return new WaitForSeconds(2);
        yield return state = State.Skill1;
        yield return new WaitUntil(() => !isSkill);
        yield return new WaitForSeconds(2);
        yield return state = State.Skill3;
        yield return new WaitUntil(() => !isSkill);
        yield return new WaitForSeconds(2);
        yield return state = State.Skill2;
        yield return new WaitUntil(() => !isSkill);
    }

    IEnumerator TVLight(float TV1, float TV2)
    {
        while (Mathf.Abs(TVButton[0].eulerAngles.z - TV1) > 0.1f)
        {
            if (TVButton[0].eulerAngles.z < TV1)
                TVButton[0].Rotate(Vector3.forward, rotateSpeed);
            else
                TVButton[0].Rotate(Vector3.back, rotateSpeed);
        }

        while (Mathf.Abs(TVButton[1].eulerAngles.z - TV2) > 0.1f)
        {
            if (TVButton[1].eulerAngles.z < TV2)
                TVButton[1].Rotate(Vector3.forward, rotateSpeed);
            else
                TVButton[1].Rotate(Vector3.back, rotateSpeed);
        }
        yield return new WaitForSeconds(1);
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
        yield return StartCoroutine(TVLight(0, 90));
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
        InvokeRepeating("Shoot", 1, 2);     // 散弹发射间隔
        yield return state = State.Idle;
        yield return new WaitWhile(() => location.Find("LocatePos").gameObject.activeInHierarchy);
        isSkill = false;
        StopCoroutine(Locate());
    }

    private void Shoot()
    {
        switch (skill)
        {
            case 0:
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

                bulletClone = Instantiate(
                    bossBullet, weaponPoint.position, Quaternion.identity);
                bulletClone.GetComponent<BossEnemyBullet>().dir = location.Find("Dir").position;
                GameController.Instance.BulletLookAt(bulletClone.transform, location.Find("Dir").position);
                break;
            case 3:
                bulletClone = Instantiate(
                    bossBullet, weaponPoint.position, Quaternion.identity);
                bulletClone.GetComponent<BossEnemyBullet>().dir = (weaponPoint.position - trigger.position).normalized * 10;
                GameController.Instance.BulletLookAt(bulletClone.transform, (weaponPoint.position - trigger.position).normalized);
                break;
        }
    }

    IEnumerator Jump()
    {
        isSkill = true;
        yield return StartCoroutine(TVLight(90, 90));
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
        yield return state = State.Idle;
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

        for (int i = 0; i < summonTime; i++)
        {
            if (state == State.Skill1 || state == State.Skill3)
                break;

            Vector3 pos = device.transform.GetChild(0).position;
            yield return pos.x = Random.Range(limit[0].x, limit[1].x);
            GameObject summon = Instantiate(bossSummon, device.transform.GetChild(0));
            summon.transform.position = pos;
            summon.GetComponent<BossEnemy>().limitY =
                device.transform.Find("LimitY").position.y;
            yield return new WaitForSeconds(1);
        }
        yield return bossSkate.GetComponent<BossSkate>().childReady = true;
        StopCoroutine(Summon());
    }

    IEnumerator BulletRain()
    {
        isSkill = true;
        yield return StartCoroutine(TVLight(90, 0));
        yield return StartCoroutine(Back(player));
        yield return new WaitWhile(
            () => anim.GetCurrentAnimatorStateInfo(0).IsName("BossReturn"));
        trigger.transform.position = transform.Find("TriggerPos").position;
        bulletTrigger.SetActive(true);
        anim.SetFloat("Angle", 0);
        yield return 1;
        Shoot();
        trigger.Translate(new Vector3(transform.lossyScale.x > 0 ? -1 : 1, 1) * 0.05f);
        yield return new WaitUntil(() => isTrigger);
        trigger.GetComponent<BossBulletTrigger>().enabled = true;
        anim.SetFloat("Angle", 0.5f);
        yield return state = State.Idle;
        //trigger.GetComponent<BossBulletTrigger>().enabled = false;
        StopCoroutine(BulletRain());
    }

    IEnumerator Back(Transform target)
    {
        if (transform.position.x > target.position.x && transform.lossyScale.x < 0 || transform.position.x < target.position.x && transform.lossyScale.x > 0)
            yield return StartCoroutine(Anim("Return"));
    }

    private void Return()
    {
        float x = -transform.localScale.x;
        transform.localScale = new Vector3(x, transform.localScale.y);
        skate.localScale = new Vector3(x, skate.localScale.y);
    }

    private void HideSkate(int i)
    {
        skate.GetComponentInChildren<SpriteRenderer>().enabled =
            i.Equals(0) ? true : false;
    }

    private void Dead()
    {
        GameController.isBoss = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == device.gameObject)
        {
            deAnim.SetInteger("Step", 2);
            StartCoroutine(Summon());
        }
    }
}
