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
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.transform.tag)
        {
            case "Trap":
                //if (!ctr.isJump)
                Death();
                break;
            case "Injurant":
                Death();
                break;
            case "Collection":
                collision.gameObject.GetComponent<DestroyController>().enabled = true;
                if (collision.gameObject == ColliNameManager.Instance.Art)
                    ColliNameManager.Instance.MapPacMan.GetComponent<PacMan>().speed = 10;//给吃豆人速度
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
