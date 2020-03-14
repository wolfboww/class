using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneManager : MonoBehaviour
{
    private BoxCollider2D col;
    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<BoxCollider2D>().bounds.min.y >= transform.position.y + col.bounds.extents.y)
            col.isTrigger = false;
    }
}
