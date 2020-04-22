using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class CollisionController : MonoBehaviour
{
    private Animator anim;
    private AudioSource au;
    private Rigidbody2D rig;
    private MoveController ctr;

    void Start()
    {
        anim = GetComponent<Animator>();
        au = GetComponent<AudioSource>();
        rig = GetComponent<Rigidbody2D>();
        ctr = GetComponent<MoveController>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (IfBullet.bemask)
            return;

        switch (collision.transform.tag)
        {
            case "Enemy":
                Death();
                break;
            case "Bounce":
                if (!ctr.isJump)
                {
                    rig.velocity = (transform.localScale.x > 0 ? Vector2.right : Vector2.left) * ctr.bounceForce;
                    rig.AddForce(Vector2.up * ctr.jumpForce);
                }
                break;
            case "Button":
                if (collision.gameObject.GetComponent<AudioSource>())
                    collision.gameObject.GetComponent<AudioSource>().Play();

                if (collision.gameObject == ColliNameManager.Instance.RotateButton)
                    collision.transform.GetComponent<Rotate>().enabled = true;
                else if (collision.gameObject == ColliNameManager.Instance.DesTrapButton)
                    collision.gameObject.GetComponentInChildren<DestroyController>().enabled = true;
                break;
            case "Bullet":
                if (collision.gameObject.GetComponent<BulletController>())
                {
                    if (collision.gameObject.GetComponent<BulletController>().playerBullet)
                        return;
                }
                else
                    collision.gameObject.GetComponent<DestroyController>().enabled = true;
                Death();
                break;
            case "Collection":
                collision.gameObject.GetComponent<DestroyController>().enabled = true;
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.transform.tag)
        {
            case "Trap":
                if (!ctr.isJump)
                    Death();
                else    //被踩蘑菇
                {
                    au.clip = ColliNameManager.Instance.enemy1;
                    au.Play();

                    rig.velocity = Vector3.up * ctr.bounceForce;
                    Animator eAnim = collision.gameObject.GetComponent<Animator>();
                    eAnim.SetTrigger("Dead");

                }
                break;
            case "Injurant":
                Death();
                break;
            case "Collection":
                collision.gameObject.GetComponent<DestroyController>().enabled = true;
                if (collision.gameObject == ColliNameManager.Instance.Gun)
                {
                    GetComponent<AnimatorController>().GetBuff(1);
                    GetComponent<Animator>().SetBool("GetGun", true);
                    GetComponent<MoveController>().bullets.Add(ColliNameManager.Instance.ElseBullet);
                }
                else if (collision.gameObject == ColliNameManager.Instance.Art)
                {
                    GetComponent<AnimatorController>().GetBuff(2);
                    GetComponent<MoveController>().bullets.Add(ColliNameManager.Instance.IfBullet);
                }
                break;
            case "Boundary":
                GameController.Instance.ChangeMap();
                GameObject[] sprites = collision.gameObject.GetComponent<Boundary>().Sprites;
                if (sprites.Length > 0)
                    foreach (var item in sprites)
                        if (!item.activeInHierarchy)
                            item.gameObject.SetActive(true);
                break;
            case "Button":
                foreach (var item in ColliNameManager.Instance.AnimBoundary)
                {
                    if (collision.gameObject == item)
                        collision.gameObject.GetComponent<Animator>().SetTrigger("Do");
                }
                break;
        }
    }

    private void Death()
    {
        anim.speed = 1;
        anim.SetTrigger("Dead");
        rig.constraints = RigidbodyConstraints2D.FreezeAll;
    }
}
