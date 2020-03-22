using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallVeloc : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GetComponent<Velocity>().enabled = true;
    }
}
