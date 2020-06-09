using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bean : MonoBehaviour
{
    private PacMan pacMan;
    private GameObject[] beans;
    private Coroutine next;

    void Start()
    {
        pacMan = ColliNameManager.Instance.MapPacMan.GetComponentInChildren<PacMan>();
        beans = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            beans[i] = transform.GetChild(i).gameObject;
        }
    }

    void Update()
    {
        if (ActiveBean())
        {
            if (next == null)
                next = StartCoroutine(NextMap());
        }
    }

    public IEnumerator NextMap()
    {
        yield return new WaitForSecondsRealtime(2);
        ColliNameManager.Instance.MainCamera.gameObject.SetActive(true);
        GameController.Instance.ChangeMap();
        GameController.Instance.player.transform.SetParent(null);
        GameController.Instance.player.transform.position
            = GameController.Instance.revivePoint.position;
    }

    public void ReStart()
    {
        foreach (var item in beans)
        {
            item.SetActive(true);
            if (item.GetComponent<MaskControl>())
                Destroy(item.GetComponent<MaskControl>());
        }
        pacMan.InitialBean();
    }

    private bool ActiveBean()
    {
        for (int i = 0; i < beans.Length; i++)
        {
            if (beans[i].activeInHierarchy)
                return false;
        }
        return true;
    }

}