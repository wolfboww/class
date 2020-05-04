using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sewer : MonoBehaviour
{
    public int life;

    void Update()
    {
        if (life <= 0)
            GetComponent<Animator>().SetTrigger("Get");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
            life--;
    }
}
