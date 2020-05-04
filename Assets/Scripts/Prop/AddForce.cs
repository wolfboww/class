using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddForce : MonoBehaviour
{
    public enum Dir
    {
        up,down,left,right
    }
    public Dir dir = Dir.up;
    public float force;

    private Vector3 Direction()
    {
        switch (dir)
        {
            case Dir.up:
                return Vector3.up;
            case Dir.down:
                return Vector3.down;
            case Dir.left:
                return Vector3.left;
            case Dir.right:
                return Vector3.right;
        }
        return Vector3.up;
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<Rigidbody2D>().AddForce(Direction() * force);
        }
    }
}
