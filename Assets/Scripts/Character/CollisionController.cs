using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
public class CollisionController : MonoBehaviour
{
    public static int life;
    public static AsyncOperation async;
    [HideInInspector]
    public Coroutine return2d;
    private Coroutine account;

    private Animator anim;
    private AudioSource au;
    private MoveController ctr;

    private bool canLoseHP = true;
    private float timer = 0;

    void Start()
    {
        anim = GetComponent<Animator>();
        au = GetComponent<AudioSource>();
        ctr = GetComponent<MoveController>();
    }

    private void Update()
    {
        if (!canLoseHP && life > 0)
        {
            timer += Time.deltaTime;
            if (timer > 1)
            {
                canLoseHP = true;
                timer = 0;
            }
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (IfBullet.bemask)
            return;

        if (collision.gameObject == ColliNameManager.Instance.Princess)
            StartCoroutine(GameController.Instance.ResetAnim(collision.gameObject.GetComponentInParent<Animator>(), "Get"));

        switch (collision.transform.tag)
        {
            case "Enemy":
                StartCoroutine(LoseHP());
                break;
            case "Bounce":
                if (!ctr.isJump)
                {
                    GetComponent<Rigidbody2D>().velocity = (transform.localScale.x > 0 ? Vector2.right : Vector2.left) * ctr.bounceForce;
                    GetComponent<Rigidbody2D>().AddForce(Vector2.up * ctr.jumpForce);
                }
                break;
            case "Button":
                if (collision.gameObject.GetComponent<AudioSource>())
                    collision.gameObject.GetComponent<AudioSource>().Play();

                if (collision.gameObject == ColliNameManager.Instance.RotateButton)
                    collision.transform.GetComponent<Rotate>().enabled = true;
                else if (collision.gameObject == ColliNameManager.Instance.DesTrapButton)
                    collision.transform.GetChild(0).gameObject.SetActive(false);
                break;
            case "Bullet":
                if (collision.gameObject.GetComponent<BulletController>())
                {
                    if (collision.gameObject.GetComponent<BulletController>().playerBullet)
                        return;
                }
                else
                    collision.gameObject.GetComponent<DestroyController>().enabled = true;

                StartCoroutine(LoseHP());
                break;
            case "Collection":
                Collection(collision.gameObject);
                break;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        switch (collision.transform.tag)
        {
            case "Trap":
                if (!ctr.isJump)
                {
                    StartCoroutine(LoseHP());
                }
                else    //被踩蘑菇
                {
                    au.clip = ColliNameManager.Instance.enemy1;
                    au.Play();

                    GetComponent<Rigidbody2D>().velocity = Vector3.up * ctr.bounceForce;
                    Animator eAnim = collision.gameObject.GetComponent<Animator>();
                    eAnim.SetTrigger("Dead");

                }
                break;
            case "Injurant":
                StartCoroutine(LoseHP());
                break;
            case "Collection":
                if (!collision.gameObject.GetComponent<DestroyController>())
                    return;

                collision.gameObject.GetComponent<DestroyController>().enabled = true;
                if (collision.gameObject == ColliNameManager.Instance.Gun)
                {
                    au.clip = ColliNameManager.Instance.getProp;
                    au.Play();
                    GetComponent<AnimatorController>().GetBuff(1);
                    GetComponent<Animator>().SetBool("GetGun", true);
                    if (!GetComponent<MoveController>().bullets.Contains(ColliNameManager.Instance.ElseBullet))
                    {
                        GetComponent<MoveController>().bullets.Add(ColliNameManager.Instance.ElseBullet);
                        ColliNameManager.Instance.Mouse.SetActive(true);
                        BulletUI.bulletNum++;
                    }
                }
                else if (collision.gameObject == ColliNameManager.Instance.Art)
                {
                    au.clip = ColliNameManager.Instance.getProp;
                    au.Play();
                    GetComponent<AnimatorController>().GetBuff(2);
                    if (!GetComponent<MoveController>().bullets.Contains(ColliNameManager.Instance.IfBullet))
                    {
                        GetComponent<MoveController>().bullets.Add(ColliNameManager.Instance.IfBullet);
                        BulletUI.bulletNum++;
                    }
                }
                else if (collision.gameObject.name == "Light(Clone)")
                {
                    au.clip = ColliNameManager.Instance.getProp;
                    au.Play();
                    GetComponent<AnimatorController>().GetBuff(3);
                    BulletUI.light = true;
                    StartCoroutine(Account());
                }
                else
                    Collection(collision.gameObject);
                break;
            case "Boundary":
                if (anim.GetFloat("Edition") < 0.1f)
                {
                    GameController.Instance.ChangeMap();
                    GameObject[] sprites = collision.gameObject.GetComponent<Boundary>().Sprites;
                    if (sprites.Length > 0)
                        foreach (var item in sprites)
                            if (!item.activeInHierarchy)
                                item.gameObject.SetActive(true);
                }
                else
                {
                    GameController.Instance.ActiveCam().GetComponent<CameraController>().boundary[1] = collision.transform.parent.GetChild(int.Parse(collision.gameObject.name));
                    RevivePoint.edition = int.Parse(collision.gameObject.name) + 3;
                }
                break;
            case "Button":
                foreach (var item in ColliNameManager.Instance.AnimBoundary)
                {
                    if (collision.gameObject == item)
                        collision.gameObject.GetComponent<Animator>().SetTrigger("Do");
                }
                break;
            case "Account":
                if (account != null)
                    return;
                account = StartCoroutine(Account());
                break;
        }
    }

    private void Collection(GameObject collision)
    {
        collision.GetComponent<DestroyController>().enabled = true;
        Instantiate(ColliNameManager.Instance.GetCollection, collision.transform.position, Quaternion.identity);
        au.clip = ColliNameManager.Instance.getCollection;
        au.Play();
        GameController.collectNum++;
        GameController.collectAccountNum++;
    }

    public IEnumerator Account()
    {
        ColliNameManager.Instance.account.SetActive(true);
        yield return new WaitUntil(() => AccountUI.go);
        anim.SetFloat("Edition", anim.GetFloat("Edition") + 1);
        if (anim.GetFloat("Edition") <= 2)
        {
            GameController.Instance.ChangeMap();
            transform.position = GameController.Instance.revivePoint.position;
            ColliNameManager.Instance.Loading.SetActive(true);
            yield return new WaitUntil(() => Loading.loading);
            Loading.loading = false;
            anim.SetTrigger("Show");
        }
        else
        {
            async = SceneManager.LoadSceneAsync("Anim");
            Action.isOver = true;
        }
        yield return 1;
        account = null;
    }

    IEnumerator LoseHP()
    {
        if (IfBullet.bemask)
            yield break;
        if (!canLoseHP)
            yield break;

        life--;
        canLoseHP = false;
        if (life > 0)
        {
            anim.SetTrigger("LoseHP");
            au.clip = ColliNameManager.Instance.loseHP;
            au.Play();
            yield break;
        }

        if (return2d != null)
            yield break;
        return2d = StartCoroutine(Return2D());
    }

    public IEnumerator Return2D()
    {
        if (!GetComponent<Rigidbody2D>())
        {
            GetComponent<MoveController>().virtual3D = false;
            transform.position =
                new Vector3(transform.position.x, transform.position.y, 0);
            yield return new WaitUntil(() => GetComponent<Rigidbody2D>());
        }
        yield return 1;
        anim.speed = 1;
        anim.SetTrigger("Dead");
        au.clip = ColliNameManager.Instance.playerDead;
        au.Play();
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        yield return StartCoroutine(GameController.Instance.Language(transform, "-_-", "•︵•"));
        return2d = null;
    }
}
