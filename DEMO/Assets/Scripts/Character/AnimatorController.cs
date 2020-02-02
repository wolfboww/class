using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    public GameObject bullet;

    private Animator anim;
    private Rigidbody2D rig;
    private Transform weaponPoint;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rig = GetComponent<Rigidbody2D>();
        weaponPoint = transform.Find("WeaponPoint");
    }

    public void Shoot()
    {
        Instantiate(bullet, weaponPoint);
    }

    public void Dead()
    {
        anim.ResetTrigger("Dead");
        rig.constraints = RigidbodyConstraints2D.FreezeRotation;
        transform.position = GameController.Instance.revivePoint.position;
    }
}
