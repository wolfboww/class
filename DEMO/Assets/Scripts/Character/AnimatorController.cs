using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    public GameObject bullet;

    private Animator anim;
    private Transform weaponPoint;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        weaponPoint = transform.Find("WeaponPoint");
    }

    public void Shoot()
    {
        Instantiate(bullet, weaponPoint);
    }

    public void Jump(float speed)
    {
        anim.speed = speed;
    }
}
