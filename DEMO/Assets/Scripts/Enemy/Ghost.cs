using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Ghost : MonoBehaviour
{
    public enum Status
    {
        Attack, Escape, Die, None
    }

    public float ghostType;
    public bool canDead;
    //[HideInInspector]
    public Status status = Status.Die;

    private AIPath aiPath;
    private Animator anim;

    private int dir = 0;
    private int life;
    private int iNum;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        aiPath = GetComponent<AIPath>();
        anim.SetFloat("Type", ghostType);
        life = GhostManager.ghostLife;
    }

    // Update is called once per frame
    void Update()
    {
        if (life <= 0 && canDead)
            status = Status.Die;

        StatusController();
        MoveDir();
    }

    private void StatusController()
    {
        switch (status)
        {
            case Status.Attack:
                GetComponent<AIPath>().enabled = true;
                break;
            case Status.Escape:
                break;
            case Status.Die:
                GetComponent<AIPath>().enabled = false;
                iNum = GhostManager.Instance.ReBorn();
                transform.position = GhostManager.Instance.initialPos[iNum].pos.position;
                GhostManager.Instance.initialPos[iNum].timer = 0;
                GhostManager.Instance.initialPos[iNum].isUsed = true;
                life = GhostManager.ghostLife;
                status = Status.None;
                break;
            case Status.None:
                if (!GhostManager.Instance.initialPos[iNum].isUsed)
                    status = Status.Attack;
                break;
        }
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
        if (collision.gameObject.tag == "Bullet")
            life--;
    }
}
