using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    private LineRenderer line;
    private Transform lineStart;

    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
        lineStart = transform.parent.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        line.SetPosition(0, transform.GetChild(1).position);
        line.SetPosition(1, lineStart.position);
    }

}
