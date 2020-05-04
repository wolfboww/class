using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : MonoBehaviour
{
    public float speed = 1;
    public bool playerChange;
    public bool horizontal;

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
        if (horizontal)
        {
            if (moveto)
            {
                if (Mathf.Abs(transform.position.x - target.x) > 0.1f)
                    transform.Translate((target - transform.position).normalized * 0.35f);
                if (Mathf.Abs(transform.position.x - target.x) <= 0.5f)
                {
                    if (playerChange)
                        GameController.Instance.player.transform.position = transform.GetChild(1).position;
                    moveback = true;
                    moveto = false;
                }
            }
            if (moveback && Mathf.Abs(transform.position.x - target.x) > 0.1f)
                transform.position = Vector3.MoveTowards(transform.position, origin, speed * Time.deltaTime);
            else
                moveback = false;
        }
        else
        {
            if (moveto)
            {
                if (Mathf.Abs(transform.position.y - target.y) > 0.1f)
                    transform.Translate((target - transform.position).normalized * 0.35f);
                if (Mathf.Abs(transform.position.y - target.y) <= 0.5f)
                {
                    if (playerChange)
                        GameController.Instance.player.transform.position = transform.GetChild(1).position;
                    moveback = true;
                    moveto = false;
                }
            }

            if (moveback && Mathf.Abs(transform.position.y - target.y) > 0.1f)
                transform.position = Vector3.MoveTowards(transform.position, origin, speed * Time.deltaTime);
            else
                moveback = false;
        }

        if (GameController.isRevive)
            transform.position = origin;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
            moveto = true;
    }
}
