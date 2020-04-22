using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    [HideInInspector]
    public GameObject bullet;

    private Animator anim;
    private AudioSource au;
    private Rigidbody2D rig;
    private Transform weaponPoint;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        au = transform.Find("WeaponPoint").GetComponent<AudioSource>();
        rig = GetComponent<Rigidbody2D>();
        weaponPoint = transform.Find("WeaponPoint");
    }

    public void Shoot()
    {
        if (bullet)
        {
            Instantiate(bullet, weaponPoint);
            au.Play();
        }
    }

    public void Dead()
    {
        anim.ResetTrigger("Dead");
        rig.constraints = RigidbodyConstraints2D.FreezeRotation;
        transform.position = GameController.isBoss ? GameController.Instance.reviveBossPoint.position : GameController.Instance.revivePoint.position;
        if (GameController.isBoss)
            ThirdCamera.gameOver = true;
    }

    public void GetBuff(int i)
    {
        anim.SetFloat("GetBuff", i);
        rig.constraints = i.Equals(0) ?
            RigidbodyConstraints2D.FreezeRotation : RigidbodyConstraints2D.FreezeAll;
    }
}
