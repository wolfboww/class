using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monkey : MonoBehaviour
{
    public float attackCD;

    private Animator anim;
    private Transform playerPos;

    private bool canAttack = true;
    private float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        playerPos = transform.GetChild(0).Find("PlayerPos");
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("CanAttack", canAttack);

        if (!canAttack)
        {
            timer += Time.deltaTime;
            canAttack = timer >= attackCD ? true : false;
        }
        else
            timer = 0;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("MonkeyIdle"))
                anim.SetTrigger("Find");
            else
                anim.SetBool("Get", true);

        }
    }

    private void ResetAnim()
    {
        anim.ResetTrigger("Find");
        anim.SetBool("Get", false);
        timer = 0;
    }

    private void Attack(int i)
    {
        Transform parent = i.Equals(0) ? playerPos : null;
        GameController.Instance.player.transform.SetParent(parent);
    }
}
