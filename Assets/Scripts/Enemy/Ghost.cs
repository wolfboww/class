using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using DG.Tweening;

public class Ghost : MonoBehaviour
{
    public enum Status
    {
        Attack, Escape, Die, Return, None
    }

    public int life;
    public float ghostType;
    public bool canDead;
    //[HideInInspector]
    public Status status = Status.None;

    private AIPath aiPath;
    private Animator anim;
    private Transform target;

    private int dir = 0;
    private int iNum;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        aiPath = GetComponent<AIPath>();
        target = GetComponent<AIDestinationSetter>().target;
        life = GhostManager.ghostLife;
    }

    // Update is called once per frame
    void Update()
    {
        if (life <= 0 && canDead)
            status = Status.Die;
        else if (life < 3 && !canDead)
            status = Status.Escape;

        if (ColliNameManager.Instance.MapPacMan.transform.GetChild(0).GetChild(0).childCount > 0)
            status = Status.Escape;

        anim.SetFloat("Type", ghostType);
        StatusController();
        MoveDir();
    }

    private void StatusController()
    {
        switch (status)
        {
            case Status.Attack:
                anim.SetBool("isDead", false);
                GetComponent<AIPath>().enabled = true;
                break;
            case Status.Escape:
                StartCoroutine(Escape());
                break;
            case Status.Die:
                anim.SetBool("isDead", true);
                GetComponent<AIPath>().enabled = false;
                iNum = GhostManager.Instance.ReBorn();
                GetComponentInChildren<GhostFX>().Revive();

                transform.position = GhostManager.Instance.initialPos[iNum].pos.position;
                GhostManager.Instance.initialPos[iNum].timer = 0;
                GhostManager.Instance.initialPos[iNum].isUsed = true;
                life = GhostManager.ghostLife;
                status = Status.Return;
                break;
            case Status.Return:
                GetComponent<AIDestinationSetter>().target = target;
                if (!GhostManager.Instance.initialPos[iNum].isUsed)
                    status = Status.Attack;
                break;
        }
    }

    private void MoveDir()
    {
        if (Mathf.Abs(aiPath.desiredVelocity.x) >= Mathf.Abs(aiPath.desiredVelocity.y))
        {
            if (aiPath.desiredVelocity.x <= -0.01f)
                dir = 0;
            else if (aiPath.desiredVelocity.x >= 0.01f)
                dir = 1;
        }
        else
        {
            if (aiPath.desiredVelocity.y >= 0.01f)
                dir = 2;
            else if (aiPath.desiredVelocity.y <= -0.01f)
                dir = 3;
        }

        GetComponentInChildren<SpriteRenderer>().flipX = dir == 1 ? true : false;
        anim.SetInteger("Dir", dir);
    }

    IEnumerator Escape()
    {
        yield return 0;
        yield return GetComponent<AIDestinationSetter>().target = EscapePos();
        life = GhostManager.ghostLife;
        status = Status.Attack;
        yield return new WaitForSeconds(GhostManager.Instance.reBackTime * 2.5f);
        yield return GetComponent<AIDestinationSetter>().target = target;

        GetComponentInChildren<GhostFX>().Revive();
        StopCoroutine(Escape());
    }

    private Transform EscapePos()
    {
        float[] distance = new float[4];
        for (int i = 0; i < distance.Length; i++)
            distance[i] = Vector2.Distance(transform.position,
                GhostManager.Instance.escapePos[i].position);

        int index = 0;
        float maxDis = distance[0];
        for (int i = 0; i < distance.Length; i++)
        {
            if (distance[i] > maxDis)
            {
                maxDis = distance[i];
                index = i;
            }
        }

        return GhostManager.Instance.escapePos[index];
    }

}
