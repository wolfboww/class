using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSkate : MonoBehaviour
{
    public static bool canGet;
    public static bool isCol;
    [HideInInspector]
    public bool childReady = false;
    public Transform skatePos;

    private Rigidbody2D rb;
    private Transform player;
    private Vector3 dis;

    // Start is called before the first frame update
    void Start()
    {
        canGet = false;
        isCol = false;

        rb = GetComponent<Rigidbody2D>();
        player = GameController.Instance.player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (childReady && Boss01.summonChildCount <= 0 
            || ThirdCamera.gameOver || !GameController.isBoss)
        {
            isCol = false;
            StartCoroutine(Return());
        }

        if (transform.root.tag.Contains("Player"))
            transform.localPosition = Vector3.zero;
    }

    IEnumerator Return()
    {
        yield return StartCoroutine(Translate(skatePos));
        rb.mass = 100;
        Boss01.isSkill = false;
        MoveController.canShoot = true;
        childReady = false;
    }

    IEnumerator Translate(Transform target)
    {
        while (Vector3.Distance(transform.position, target.position) > 1f)
        {
            yield return transform.position =
                Vector3.MoveTowards(transform.position, target.position, 1f);
        }
        transform.SetParent(target);
        yield return transform.localPosition = Vector3.zero;
        rb.velocity = Vector3.zero;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!canGet)
            return;

        if (collision.gameObject.tag == "Player")
        {
            isCol = true;
            MoveController.canShoot = false;
            rb.mass = 0;
            StartCoroutine(Translate(player.Find("SkatePos")));
        }
    }
}
