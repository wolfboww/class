using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletcurtain : MonoBehaviour
{
    public GameObject prefabbullet;
    public float fireRate = 0.5F;
    private float nextFire = 0.0F;
    public float speed = 5f;
    float num = 0;
    private List<GameObject>  objbullet;
    
    void Start()
    {
        objbullet = new List<GameObject>();
    }
    private void Update()
    {
        if (Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            GameObject bullet = Instantiate(prefabbullet, transform.position, transform.rotation);
            Rigidbody2D clone = bullet.GetComponent<Rigidbody2D>();
            objbullet.Add(bullet);
            num ++;
            if (clone != null)
            {
                clone.velocity = transform.TransformDirection(Vector3.forward * speed);
            }
            if(num >3)
            {
                Destroy(objbullet[0]);
                objbullet.RemoveAt(0);
            }
        }
    }
    /*给子弹的
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "player")
        {
            Destroy(other.gameObject);
        }
    } */
}
