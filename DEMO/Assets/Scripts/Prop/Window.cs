using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : MonoBehaviour
{
    public float speed = 1;

    private Vector3 origin;
    private Vector3 target;

    private bool moveto = false;
    private bool moveback = false;
    // Start is called before the first frame update
    void Start()
    {
        origin = transform.position;
        target = transform.GetChild(0).position;
    }

    // Update is called once per frame
    void Update()
    {
        if (moveto && transform.position.x < target.x)
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        else if (transform.position.x >= target.x)
        {
            GameController.Instance.player.transform.position = transform.GetChild(1).position;
            moveback = true;
            moveto = false;
        }

        if (moveback && transform.position.x >= origin.x)
            transform.position = Vector3.MoveTowards(transform.position, origin, speed * Time.deltaTime);
        else
            moveback = false;

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
            moveto = true;
    }
}
