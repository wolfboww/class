using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Sweat : MonoBehaviour
{
    [HideInInspector]
    public bool fade;

    private void OnEnable()
    {
        fade = false;
        StartCoroutine(Fade());
    }

    private void Update()
    {
        if (!GameController.Instance.player.transform.Find("WeaponPoint").GetChild(0).gameObject.activeInHierarchy)
            fade = true;
    }

    IEnumerator Fade()
    {
        float a = GetComponent<SpriteRenderer>().color.a;
        GetComponent<SpriteRenderer>().DOFade(1, 1);
        yield return new WaitUntil(() => fade);
        GetComponent<SpriteRenderer>().DOFade(a, 1);
        yield return new WaitUntil(() => Mathf.Abs(GetComponent<SpriteRenderer>().color.a - a) <= 0.1f);
        Destroy(this);
    }
}
