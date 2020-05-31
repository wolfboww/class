using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GhostFX : MonoBehaviour
{
    public void Revive()
    {
        if (DOTween.IsTweening(transform))
            transform.DOKill();
        transform.localScale = Vector3.one;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            //if (!DOTween.IsTweening(transform))
            {
                GetComponentInParent<Ghost>().life--;
                transform.DOScale(transform.localScale * 0.8f, 1);
            }
        }

    }
}
