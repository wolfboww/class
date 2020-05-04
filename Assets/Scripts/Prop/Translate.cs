using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Translate : MonoBehaviour
{
    public float moveSpeed = 1f;
    public bool isProp;

    private Vector3 moveto;
    private Vector3 origin;
    private Vector3 target;

    // Start is called before the first frame update
    void Start()
    {
        origin = transform.position;
        target = transform.GetChild(0).position;
        moveto = target;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, moveto) <= 0.05f)
            moveto = moveto == target ? origin : target;
        else
            transform.position = Vector3.MoveTowards(transform.position, moveto, moveSpeed * Time.deltaTime);

        if (GameController.isRevive)
        {
            transform.position = origin;
            if (isProp)
                this.enabled = false;
        }
    }

}
