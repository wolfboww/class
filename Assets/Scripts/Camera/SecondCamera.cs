using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using DG.Tweening;

public class SecondCamera : MonoBehaviour
{
    private Camera firstCamera;
    private Camera secondCamera;

    void Start()
    {
        firstCamera = ColliNameManager.Instance.MainCamera;
        secondCamera = ColliNameManager.Instance.SecondCamera;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            StartCoroutine(SCamera());
    }

    IEnumerator SCamera()
    {
        Destroy(GetComponent<EdgeCollider2D>());
        firstCamera.gameObject.SetActive(false);
        secondCamera.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        yield return secondCamera.DOOrthoSize(11, 3);
        for (int i = 0; i < GhostManager.Instance.enemy.childCount; i++)
            GhostManager.Instance.enemy.GetChild(i).GetComponent<Ghost>().status = Ghost.Status.Die;
        yield return new WaitForSeconds(3);
        ColliNameManager.Instance.MapPacMan.GetComponent<AIPath>().enabled = true;
    }
}
