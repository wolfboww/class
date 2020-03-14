using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public Transform rotateObj;
    public float rotateAngle;

    // Update is called once per frame
    void Update()
    {
        if (rotateObj.localEulerAngles.z <= rotateAngle)
            rotateObj.Rotate(Vector3.forward, 22 * Time.deltaTime);
    }

}
