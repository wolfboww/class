using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootButton : MonoBehaviour
{
    public enum Target
    {
        transPlat, rotatePlat, None
    }
    public GameObject targetObj;
    public Target target = Target.None;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Bullet")
        {
            switch (target)
            {
                case Target.transPlat:
                    targetObj.GetComponent<Translate>().enabled = true;
                    break;
                case Target.rotatePlat:
                    targetObj.GetComponent<Rotate>().enabled = true;
                    break;
            }
        }
    }
}
