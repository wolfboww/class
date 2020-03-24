using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallVeloc : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Plane")
            GetComponent<Velocity>().enabled = true;
    }
}
