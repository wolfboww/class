using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lift : MonoBehaviour
{
    public float speed;
    public bool buttonController;

    private GameObject light;
    private Color initialColor;
    private Vector3 lastPos;
    private Vector3 newPos;
    private Vector3 targetPos;
    private bool isPlayerOn;

    private float timer = -1;
    private float rebackTime = 4;
    private float offset = 0.05f;

    // Start is called before the first frame update
    void Start()
    {
        light = transform.Find("light1").gameObject;
        initialColor = light.GetComponent<SpriteRenderer>().color;
        lastPos = transform.position;
        newPos = transform.GetChild(0).position;
    }

    // Update is called once per frame
    void Update()
    {
        targetPos = isPlayerOn ? newPos : lastPos;
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

        if (!buttonController)
        {
            if (Vector3.Distance(transform.position, lastPos) < 0.1f)
                light.GetComponent<SpriteRenderer>().color = Color.white;
            else
                light.GetComponent<SpriteRenderer>().color = initialColor;
        }

        if (!timer.Equals(-1) && isPlayerOn)
        {
            timer += Time.deltaTime;
            if (timer >= rebackTime)
            {
                timer = -1;
                isPlayerOn = false;
            }
        }

        if (GameController.isRevive)
            transform.position = lastPos;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (light.GetComponent<SpriteRenderer>().color != Color.white 
            && buttonController)
            return;

        if (collision.gameObject.tag == "Player" && !isPlayerOn)
        {
            if (collision.gameObject.GetComponent<BoxCollider2D>().bounds.min.y >= GetComponent<BoxCollider2D>().bounds.max.y - offset)
            {
                isPlayerOn = true;
                timer = 0;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && isPlayerOn)
        {
            isPlayerOn = false;
            timer = -1;
        }
    }
}
