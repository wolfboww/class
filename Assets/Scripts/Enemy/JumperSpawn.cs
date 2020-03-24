using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumperSpawn : MonoBehaviour
{
    [HideInInspector]
    public Transform destroyHeight;
    public GameObject jumperObj;
    public float spawnCD;
    public float spawnRange;

    private float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        destroyHeight = transform.Find("destroyHeight");
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > spawnCD)
        {
            GameObject child = Instantiate(jumperObj, transform);
            Vector3 spawnPos = new Vector3(Random.Range(-spawnRange, spawnRange), 0, 0);
            child.transform.position += spawnPos;
            timer = 0;
        }
    }
}
