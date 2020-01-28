using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateButton : MonoBehaviour
{
    public Transform rotateObj;

    private bool rotate = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (rotate && rotateObj.localEulerAngles.z <= 30)
            rotateObj.Rotate(Vector3.forward, 22 * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player")
            rotate = true;
    }
}
