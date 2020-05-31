using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using DG.Tweening;

public class SecondCamera : MonoBehaviour
{
    public Transform[] points;
    public GameObject mask;

    private Transform pacman;
    private Camera firstCamera;
    private Camera secondCamera;
    private int i = 0;

    void Start()
    {
        pacman = ColliNameManager.Instance.MapPacMan.transform;
        firstCamera = ColliNameManager.Instance.MainCamera;
        secondCamera = ColliNameManager.Instance.SecondCamera;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            StartCoroutine(SCamera());
    }

    IEnumerator SCamera()
    {
        GetComponent<EdgeCollider2D>().isTrigger = false;
        firstCamera.gameObject.SetActive(false);
        secondCamera.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        secondCamera.transform.DOLocalMove(new Vector3(-1.6f, 0.024f, -10), 3);
        yield return secondCamera.DOOrthoSize(11, 3);
        yield return pacman.GetComponent<AIPath>().enabled = true;
        while (true)
        {
            if (Vector2.Distance(pacman.transform.position, points[i].position) > 0.1f)
                pacman.GetComponent<AIDestinationSetter>().target = points[i];
            else if (i < points.Length - 1)
                i++;
            else break;

            yield return 1;
        }
        mask.GetComponent<Animator>().SetTrigger("Mask");
        ColliNameManager.Instance.Mouse.SetActive(true);
        ColliNameManager.Instance.Mouse.GetComponent<Animator>().SetFloat("Edition", 1);
        yield return new WaitUntil(() => mask.transform.GetChild(0).childCount > 0);
        yield return pacman.GetComponent<AIDestinationSetter>().target = transform.root.Find("EnemyPos").Find("PacmanPos");
    }
}
