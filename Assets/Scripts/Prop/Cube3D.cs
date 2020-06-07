using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube3D : MonoBehaviour
{
    private bool destroy = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (destroy)
        {
            GameObject child=Instantiate(ColliNameManager.Instance.CubeAnim, transform.position, Quaternion.identity);
            Destroy(child, 2);
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.GetComponent<BulletController>())
            return;
        if (!collision.gameObject.GetComponent<BulletController>().playerBullet)
            return;

        collision.gameObject.GetComponent<DestroyController>().enabled = true;
        destroy = true;
    }
}
