using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPlat : MonoBehaviour
{
    private BoxCollider2D plat;
    private bool isTrigger;

    // Start is called before the first frame update
    void Start()
    {
        plat = transform.parent.GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        plat.isTrigger = isTrigger ? true : false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag.Contains("Player"))
            isTrigger = false;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag.Contains("Player"))
            isTrigger = true;
    }
}
