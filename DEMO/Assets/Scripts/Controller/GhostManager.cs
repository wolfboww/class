using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class GhostManager : MonoBehaviour
{
    public struct GPos
    {
        public int number;
        public Transform pos;
        public bool isUsed;
        public float timer;
    }

    [HideInInspector]
    public static int ghostLife = 3;
    [HideInInspector]
    public GPos[] initialPos = new GPos[3];
    [HideInInspector]
    public Transform[] escapePos = new Transform[4];
    [HideInInspector]
    public float reBackTime = 3;
    public Transform enemy;
    public Bean bean;

    public float reBornTime;
    public bool gameOver = false;

    private GameObject pacMan;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            initialPos[i].number = i;
            initialPos[i].pos = transform.GetChild(i);
            initialPos[i].isUsed = false;
            initialPos[i].timer = -1;
        }

        for (int i = 0; i < 4; i++)
            escapePos[i] = transform.GetChild(3).GetChild(i);

        pacMan = ColliNameManager.Instance.MapPacMan;
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var item in initialPos)
        {
            int i = item.number;
            if (initialPos[i].timer >= 0 && initialPos[i].timer < reBornTime)
                initialPos[i].timer += Time.deltaTime;
            else if (initialPos[i].timer >= reBornTime)
                initialPos[i].isUsed = false;
        }

        if (gameOver)
            StartCoroutine(GameReStart());
    }

    IEnumerator GameReStart()
    {
        for (int i = 0; i < enemy.childCount; i++)
            enemy.GetChild(i).GetComponent<Ghost>().status = Ghost.Status.Die;
        pacMan.transform.position = pacMan.GetComponentInChildren<PacMan>().rePos.position;
        pacMan.GetComponentInChildren<PacMan>().dir = PacMan.Dir.right;
        pacMan.GetComponent<AIPath>().enabled = false;
        bean.ReStart();
        yield return gameOver = false;
        yield return new WaitForSeconds(reBackTime);
        pacMan.GetComponent<AIPath>().enabled = true;
    }


    public int ReBorn()
    {
        foreach (var item in initialPos)
        {
            if (!item.isUsed)
                return item.number;
        }

        return 0;
    }

    public static GhostManager _instance;
    public static GhostManager Instance
    {
        get
        {
            return _instance;
        }
    }

    public void Awake()
    {
        _instance = this;
    }
}