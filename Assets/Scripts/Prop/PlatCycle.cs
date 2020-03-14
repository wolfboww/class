using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatCycle : MonoBehaviour
{
    public GameObject[] cycleObj;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        foreach (GameObject item in cycleObj)
        {
            if (collision.gameObject == item)
                GetComponentInParent<Rigidbody2D>().velocity *= -1;
        }
    }

}
