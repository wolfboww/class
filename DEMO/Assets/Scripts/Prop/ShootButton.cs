using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootButton : MonoBehaviour
{
    public enum Target
    {
        transPlat, rotatePlat, DesPlat, ActivePlat, AnimPlat, None
    }
    public GameObject targetObj;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Bullet")
        {
            if (anim)
                anim.SetTrigger("Get");
            switch (target)
            {
                case Target.transPlat:
                    targetObj.GetComponent<Translate>().enabled = true;
                    break;
                case Target.rotatePlat:
                    targetObj.GetComponent<Rotate>().enabled = true;
                    break;
                case Target.DesPlat:
                    if (shootNum < 5)
                        shootNum++;
                    else
                        targetObj.GetComponent<DestroyController>().enabled = true;
                    break;
                case Target.ActivePlat:
                    if (!targetObj.activeInHierarchy)
                        targetObj.SetActive(true);
                    break;
                case Target.AnimPlat:
                    targetObj.GetComponent<Animator>().SetTrigger("Do");
                    break;
            }
        }
    }
}
