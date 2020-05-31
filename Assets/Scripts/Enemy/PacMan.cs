using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class PacMan : MonoBehaviour
{
    public enum Dir
    {
        up, down, left, right
    }
    public Transform path;
    public Transform rePos;
    [HideInInspector]
    public Dir dir = Dir.right;

    private AIPath aiPath;
    private List<Transform> pathPoint = new List<Transform>();
    private List<Transform> leftPoint = new List<Transform>();

    private Vector3 hatPos;
    private float hatPosX;

    void Start()
    {
        aiPath = GetComponentInParent<AIPath>();
        hatPos = transform.GetChild(0).localScale;
        hatPosX = hatPos.x;
    }

    void Update()
    {
        MoveDir();
    }

    public void MoveDir()
    {
        if (aiPath.desiredVelocity.x <= -2f)
            dir = Dir.left;
        else if (aiPath.desiredVelocity.x >= 2f)
            dir = Dir.right;

        if (aiPath.desiredVelocity.y >= 2f)
            dir = Dir.up;
        else if (aiPath.desiredVelocity.y <= -2f)
            dir = Dir.down;

        switch (dir)
        {
            case Dir.up:
                transform.localEulerAngles = new Vector3(0, 0, 90);
                break;
            case Dir.down:
                transform.localEulerAngles = new Vector3(0, 0, -90);
                break;
            case Dir.left:
            case Dir.right:
                transform.localEulerAngles = Vector3.zero;
                break;
        }
        GetComponent<SpriteRenderer>().flipX = dir == Dir.left ? true : false;
        hatPos.x = (dir == Dir.left ? -1 : 1) * hatPosX;
        transform.GetChild(0).localScale = hatPos;
    }

    public void InitialBean()
    {
        for (int i = 0; i < path.childCount; i++)
            pathPoint.Add(path.GetChild(i));
        StartCoroutine(FindTarget());
    }

    IEnumerator FindTarget()
    {
        while (pathPoint.Count > 0)
        {
            int index = Random.Range(0, pathPoint.Count);
            Transform target = pathPoint[index];
            yield return new WaitUntil(() => target.gameObject.activeInHierarchy);
            GetComponentInParent<AIDestinationSetter>().target = target;
            yield return new WaitWhile(() => target.gameObject.activeInHierarchy);
            yield return 0;
            pathPoint = new List<Transform>();
            for (int i = 0; i < path.childCount; i++)
                if (path.GetChild(i).gameObject.activeInHierarchy)
                    pathPoint.Add(path.GetChild(i));
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //if (collision.name == "Mask")
        //    collision.transform.parent.SetParent(transform);

        switch (collision.transform.tag)
        {
            case "Bean":
                collision.gameObject.SetActive(false);
                break;
            case "Mask":
                if (collision.gameObject.GetComponent<MaskControl>())
                    GameController.Instance.Mask(gameObject);
                collision.gameObject.SetActive(false);
                break;
            case "Enemy":
                if (!transform.GetChild(0).childCount.Equals(0))
                    return;
                else if (collision.GetComponentInParent<Ghost>().ghostType != 2)
                    GhostManager.Instance.gameOver = true;
                break;
        }
    }
}