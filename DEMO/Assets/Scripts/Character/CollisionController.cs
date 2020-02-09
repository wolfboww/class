using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionController : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rig;
    private MoveController ctr;

    void Start()
    {
        anim = GetComponent<Animator>();
        rig = GetComponent<Rigidbody2D>();
        ctr = GetComponent<MoveController>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.transform.tag)
        {
            case "Enemy":
                if (!IfBullet.bemask)
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
                if (collision.gameObject == ColliNameManager.Instance.RotateButton)
                    collision.transform.GetComponent<Rotate>().enabled = true;
                else if (collision.gameObject == ColliNameManager.Instance.DesTrapButton)
                    collision.gameObject.GetComponentInChildren<DestroyController>().enabled = true;
                break;
            case "Bullet":
                Death();
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
                    Animator eAnim = collision.gameObject.GetComponent<Animator>();
                    eAnim.SetTrigger("Dead");
                    collision.GetComponent<DestroyController>().enabled = true;
                }
                break;
            case "Injurant":
                Death();
                break;
            case "Collection":
                collision.gameObject.GetComponent<DestroyController>().enabled = true;
                if (collision.gameObject == ColliNameManager.Instance.Art)
                    ColliNameManager.Instance.MapPacMan.GetComponent<PacMan>().speed = 5;//给吃豆人速度
                else if (collision.gameObject == ColliNameManager.Instance.Gun)
                {
                    GetComponent<Animator>().SetBool("GetGun", true);
                    GetComponent<MoveController>().bullets.Add(ColliNameManager.Instance.ElseBullet);
                }
                break;
            case "Boundary":
                GameController.Instance.ChangeMap();
                foreach (var item in collision.gameObject.GetComponent<Boundary>().Sprites)
                    if (!item.activeInHierarchy)
                        item.gameObject.SetActive(true);
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
