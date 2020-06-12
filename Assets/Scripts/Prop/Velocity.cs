using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Velocity : MonoBehaviour
{
    public enum Dir
    {
        Right, Left, Up, Down
    }
    public float velocity;
    public Dir dir = Dir.Down;

    private Vector3 moveDir = Vector3.down;

    // Start is called before the first frame update
    void OnEnable()
    {
        switch (dir)
        {
            case Dir.Down:
                moveDir = Vector3.down;
                break;
            case Dir.Up:
                moveDir = Vector3.up;
                break;
            case Dir.Right:
                moveDir = Vector3.right;
                break;
            case Dir.Left:
                moveDir = Vector3.left;
                break;
        }

        GetComponent<Rigidbody2D>().velocity = moveDir * velocity;
    }

    private void Update()
    {
        if(GameController.isRevive)
            GetComponent<Rigidbody2D>().velocity = moveDir * velocity;
    }
}
