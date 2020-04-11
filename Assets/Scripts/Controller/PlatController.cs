using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatController : MonoBehaviour
{
    private Vector3 scale;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player"
            && !collision.gameObject.GetComponent<MoveController>().isJump
            ||collision.transform .tag =="Enemy")
        {
            scale = collision.transform.localScale;
            collision.transform.SetParent(transform.GetChild(0));
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            collision.transform.SetParent(null);
            //if (collision.transform.parent == null)
            //collision.transform.localScale = scale;
        }
        else if (collision.transform.tag == "Enemy")
        {
            Transform parent = transform.root.Find("Enemy");
            collision.transform.SetParent(parent);
        }
    }
}
