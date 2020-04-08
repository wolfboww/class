using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ThirdCamera : MonoBehaviour
{
    private GameObject player;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        player = GameController.Instance.player;
        anim = player.GetComponent<Animator>();
    }

    IEnumerator BossAnim()
    {
        ColliNameManager.Instance.MainCamera.gameObject.SetActive(false);
        transform.Find("ThirdCamera").gameObject.SetActive(true);
        yield return 0;
        GetComponent<StopControl>().PlayerStop(1);
        yield return 1;
        player.transform.position = transform.Find("PlayerPos").position;
        player.transform.localScale = new Vector3(-1, 1, 1);
        anim.SetFloat("Shoot", 1);
        player.GetComponent<AnimatorController>().bullet = ColliNameManager.Instance.IfBullet;
        player.GetComponent<AnimatorController>().Shoot();
        //anim.Play("PlayerJumpShoot02-forward");
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1f);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(BossAnim());
    }
}
