using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootButton : MonoBehaviour
{
    public enum Target
    {
        None, transPlat, rotatePlat, DesPlat, ActivePlat, AnimPlat, HandlePlat, LiftPlat
    }
    public int life;
    public GameObject targetObj;
    public GameObject particle;
    public Target target = Target.None;

    private int shootNum = 0;
    private Animator anim;

    private void Start()
    {
        if (GetComponent<Animator>())
            anim = GetComponent<Animator>();
        else
            anim = null;
    }

    private void Delay()
    {
        targetObj.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Bullet")
        {
            if (!collision.gameObject.GetComponent<BulletController>())
                return;
            if (!collision.gameObject.GetComponent<BulletController>().playerBullet)
                return;

            //if (gameObject.layer == LayerMask.NameToLayer("Mask")
            //    && !collision.gameObject.name.Equals("IfBullet(Clone)"))
            //    return;

            if (anim && shootNum == life)
                StartCoroutine(GameController.Instance.ResetAnim(anim, "Get"));

            if (particle != null)
                Instantiate(particle, transform.position, Quaternion.identity);

            if (targetObj != null)
            {
                switch (target)
                {
                    case Target.transPlat:
                        targetObj.GetComponent<Translate>().enabled = true;
                        break;
                    case Target.rotatePlat:
                        targetObj.GetComponent<Rotate>().enabled = true;
                        break;
                    case Target.DesPlat:
                        if (shootNum < life)
                            shootNum++;
                        else
                            targetObj.SetActive(false);
                        break;
                    case Target.ActivePlat:
                        if (!targetObj.activeInHierarchy)
                            targetObj.SetActive(true);
                        break;
                    case Target.AnimPlat:
                        StartCoroutine(GameController.Instance.ResetAnim(targetObj.GetComponent<Animator>(), "Do"));
                        break;
                    case Target.HandlePlat:
                        targetObj.GetComponent<HandleMove>().handle.isPlus
                            = int.Parse(transform.name);
                        GameController.Instance.player.transform.
                            SetParent(transform.parent.GetChild(0).GetChild(0));
                        break;
                    case Target.LiftPlat:
                        targetObj.GetComponent<SpriteRenderer>().color = Color.white;
                        break;
                }
            }
        }
    }

}
