using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public GPos[] initialPos = new GPos[3];
    [HideInInspector]
    public static int ghostLife = 3;
    public GameObject enemy;
    public float reBornTime;

    private GameObject[] gObj = new GameObject[3];

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            gObj[i] = enemy.transform.GetChild(0).gameObject;
            initialPos[i].number = i;
            initialPos[i].pos = transform.GetChild(i);
            initialPos[i].isUsed = false;
            initialPos[i].timer = -1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var item in initialPos)
        {
            int i = item.number;
            if (initialPos[i].timer >= 0 && initialPos[i].timer < reBornTime)
                initialPos[i].timer += 0.1f;
            else if (initialPos[i].timer >= reBornTime)
                initialPos[i].isUsed = false;
        }
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