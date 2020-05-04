using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallVeloc : MonoBehaviour
{
    public float speed;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Plane")
            GetComponent<Rigidbody2D>().velocity = Vector2.right * speed;
    }
}
