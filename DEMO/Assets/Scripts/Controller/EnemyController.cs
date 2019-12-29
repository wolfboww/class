using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnemyDeath(GameObject Enemy, int range)
    {
        //播放死亡动画
        Enemy.GetComponent<DestroyController>().enabled = true;
    }

    public static EnemyController _instance;
    public static EnemyController Instance
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
