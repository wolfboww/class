using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneManager : MonoBehaviour
{

    private BoxCollider2D collider;
    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.position.y - collision.gameObject.GetComponent<BoxCollider2D>().bounds.size.y / 2 >= transform.position.y + collider.bounds.size.y / 2)
            collider.isTrigger = false;
    }
}
