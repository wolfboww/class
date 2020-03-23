using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IfBullet : MonoBehaviour
{
    static public bool bemask = false;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Mask")
        {
            bemask = true;
            GameController.Instance.player.transform.position = collision.transform.position;
            GameController.Instance.Mask(true,
                collision.gameObject.GetComponent<SpriteRenderer>().sprite);
        }

    }
}
