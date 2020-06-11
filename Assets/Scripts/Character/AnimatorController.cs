using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AnimatorController : MonoBehaviour
{
    [HideInInspector]
    public GameObject bullet;

    private Animator anim;
    private AudioSource au;
    private Transform weaponPoint;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        au = transform.Find("WeaponPoint").GetComponent<AudioSource>();
        weaponPoint = transform.Find("WeaponPoint");
        anim.SetTrigger("Show");
    }

    public void Shoot()
    {
        if (bullet)
        {
            GameController.Instance.ActiveCam().DOShakePosition(0.3f, 0.2f, 20, 50, false);
            if (GetComponent<Rigidbody>())
                Instantiate(bullet == ColliNameManager.Instance.ElseBullet ? ColliNameManager.Instance.ElseBullet3D : bullet, weaponPoint);
            else
                Instantiate(bullet, weaponPoint);
            au.Play();
        }
    }

    public void Dead()
    {
        anim.ResetTrigger("Dead");
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        if (IfBullet.bemask)
            GetComponent<MoveController>().BeNotMask();
        transform.position = GameController.Instance.revivePoint.position;
        GameController.isRevive = true;
        if (GameController.isBoss)
            ThirdCamera.gameOver = true;
        StartCoroutine(GameController.Instance.Language(transform, "* _ *", "ｕ︵ｕ"));
    }

    public void GetBuff(int i)
    {
        anim.SetFloat("GetBuff", i);
        GetComponent<Rigidbody2D>().constraints = i.Equals(0) ?
            RigidbodyConstraints2D.FreezeRotation : RigidbodyConstraints2D.FreezeAll;
    }
}
