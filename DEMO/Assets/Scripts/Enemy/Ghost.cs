using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Ghost : MonoBehaviour
{
    public float ghostType;
    public bool canDead;

    private AIPath aiPath;
    private Animator anim;
    private int dir = 0;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        aiPath = GetComponent<AIPath>();
        anim.SetFloat("Type", ghostType);
    }

    // Update is called once per frame
    void Update()
    {
        MoveDir();
    }

    private void MoveDir()
    {
        if (aiPath.desiredVelocity.x <= -2f)
            dir = 0;
        else if (aiPath.desiredVelocity.x >= 2f)
            dir = 1;

        if (aiPath.desiredVelocity.y >= 2f)
            dir = 2;
        else if (aiPath.desiredVelocity.y <= -2f)
            dir = 3;

        GetComponent<SpriteRenderer>().flipX = dir == 1 ? true : false;
        anim.SetInteger("Dir", dir);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
}
