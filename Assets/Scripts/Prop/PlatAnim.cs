using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatAnim : MonoBehaviour
{
    public GameObject[] enemy;

    private Animator anim;
    private Coroutine cor;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.isRevive)
            StartCoroutine(Revive());

        foreach (var item in enemy)
        {
            if (!item.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("EnemyDead"))
                return;
        }

        if (cor != null)
            return;
        cor = StartCoroutine(Anim());
    }

    IEnumerator Revive()
    {
        anim.ResetTrigger("Do");
        yield return new WaitUntil(() =>
        {
            foreach (var item in enemy)
            {
                if (item.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("EnemyDead"))
                    return false;
            }
            return true;

        });
        cor = null;

    }

    IEnumerator Anim()
    {
        anim.SetTrigger("Do");
        yield return 1;
        anim.ResetTrigger("Do");
    }
}
