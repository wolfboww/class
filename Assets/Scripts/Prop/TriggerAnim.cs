using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAnim : MonoBehaviour
{
    public bool light;
    public Animator anim;
    public string animName;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (light && collision.GetComponent<LightManager>())
            anim.SetTrigger(animName);
        else if (collision.tag.Contains("Player"))
            anim.SetTrigger(animName);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            if (collision.gameObject.GetComponent<IfBullet>())
                return;
            if (!collision.gameObject.GetComponent<BulletController>())
                return;
            if (!collision.gameObject.GetComponent<BulletController>().playerBullet)
                return;

            anim.SetTrigger(animName);
        }
    }
}
