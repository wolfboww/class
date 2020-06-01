using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CubeMove : MonoBehaviour
{
    [HideInInspector]
    public bool down = false;
    public float moveSpeed;
    public static int downIndex;

    private Transform[] child;

    // Start is called before the first frame update
    void Start()
    {
        child = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
            child[i] = transform.GetChild(i);
        downIndex = transform.childCount - 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.isRevive)
        {
            down = false;
            downIndex = transform.childCount - 1;
        }

        if (down)
            StartCoroutine(Down());
    }

    IEnumerator Down()
    {
        down = false;
        yield return new WaitForSeconds(2);
        if (downIndex >= 0)
            child[downIndex].GetComponent<CubeManager>().enabled = true;
        yield return 1;
        downIndex--;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            down = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag.Contains("Mask"))
        {
            float minY = collision.GetComponent<PolygonCollider2D>() ? collision.GetComponent<PolygonCollider2D>().bounds.min.y :
                collision.GetComponent<BoxCollider2D>().bounds.min.y;
            collision.transform.Find("Bottom").position = new Vector3(collision.transform.Find("Bottom").position.x, minY - 0.1f);
        }

        if (collision.tag.Contains("Plane") && collision.transform.parent.parent.name == "Sprite")
            collision.GetComponent<BoxCollider2D>().isTrigger = false;
    }
}
