using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LightManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Contains("Enemy"))
            if (collision.GetComponentInChildren<EnemyActive>())
                if (collision.GetComponentInChildren<EnemyActive>().canShoot)
                    collision.GetComponentInChildren<EnemyActive>().active = true;

        if (collision.gameObject.tag.Contains("Sweat"))
        {
            if (collision.gameObject.GetComponent<Sweat>())
                return;
            collision.gameObject.AddComponent<Sweat>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Contains("Sweat"))
            collision.GetComponent<Sweat>().fade = true;
    }
}
