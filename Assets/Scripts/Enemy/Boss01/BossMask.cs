using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMask : MonoBehaviour
{

    private float timer = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 10 && transform.GetChild(0).childCount <= 0)
            Destroy(gameObject);
    }
}
