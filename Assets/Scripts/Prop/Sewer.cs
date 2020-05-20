using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sewer : MonoBehaviour
{
    public int life;
    public GameObject effect;

    void Update()
    {
        if (life <= 0)
            GetComponent<Animator>().SetTrigger("Get");
    }

    void Effect()
    {
        GameObject child = Instantiate(effect, transform.position, Quaternion.identity);
        Destroy(child,child.GetComponent<ParticleSystem>().main.duration);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
            life--;
    }
}
