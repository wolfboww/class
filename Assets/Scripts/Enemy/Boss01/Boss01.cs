using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss01 : MonoBehaviour
{
    public enum State
    {
        Idle,Run,Attack,
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Return()
    {
        float x = -transform.localScale.x;
        transform.localScale = new Vector3(x, transform.localScale.y);
    }
}
