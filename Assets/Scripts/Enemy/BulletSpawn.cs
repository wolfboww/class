using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawn : MonoBehaviour
{
    public GameObject bullet;
    public float SpawnCD;
    public float bulletSpeed;

    private Vector3 dir;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        dir = (transform.GetChild(0).position - transform.position).normalized;
    }

    // Update is called once per frame
    void Update()
    {
        dir = (transform.GetChild(0).position - transform.position).normalized;
        timer += Time.deltaTime;
        if (timer >= SpawnCD)
        {
            GameObject child = Instantiate(bullet, transform);
            child.GetComponent<Rigidbody2D>().velocity = dir * (bulletSpeed.Equals(0) ? child.GetComponent<BulletController>().speed : bulletSpeed);
            timer = 0;
        }
    }
}
