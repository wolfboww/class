using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SlotMachine : MonoBehaviour
{
    public int type;
    private Animator anim;
    private int initialType;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        initialType = type;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.isRevive)
            anim.SetFloat("Type", initialType);

        type = type > 2 ? type - 3 : type;
        anim.SetFloat("Type", type);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Contains("Player"))
        {
            if (!DOTween.IsTweening(transform))
            {
                transform.DOPunchPosition(Vector3.up * 0.1f, 0.3f, 1, 0);
                type++;
            }
        }
    }
}
