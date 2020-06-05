using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualChange : MonoBehaviour
{
    public bool virtial;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Contains("Player"))
            collision.GetComponent<MoveController>().virtual3D = virtial;
    }
}
