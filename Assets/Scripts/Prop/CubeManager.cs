using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeManager : MonoBehaviour
{
    [HideInInspector]
    public Vector3 freezePos;

    // Start is called before the first frame update
    void OnEnable()
    {
        freezePos = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.down;
        transform.position = new Vector3(freezePos.x, transform.position.y);

    }
}
