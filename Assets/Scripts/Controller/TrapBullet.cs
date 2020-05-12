using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapBullet : MonoBehaviour
{
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(GetComponent<Rigidbody2D>());
        if (anim != null)
            anim.SetTrigger("Dead");
        else
            GetComponent<DestroyController>().enabled = true;
    }
}
