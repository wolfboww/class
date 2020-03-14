using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stuff : MonoBehaviour
{
    public float length;
    public float speed;

    private float dis;
    private bool flip = true;
    private Vector2 initpos;

    void Start()
    {
        initpos = gameObject.transform.position;
    }
    void Update()
    {
        dis = Mathf.Abs(GameController.Instance.player.transform.position.x - gameObject.transform.position.x);
        float reallength = Mathf.Abs(gameObject.transform.position.x - initpos.x);
        if (dis <= 2)//追主角
        {
            var dir = Vector3.Normalize(GameController.Instance.player.transform.position - transform.position);
            transform.Translate(dir * speed * 2 * Time.deltaTime);
        }
        else//巡逻
        {
            gameObject.transform.position = new Vector2(length * Mathf.Sin(Time.time * speed) + initpos.x, initpos.y);
            if (reallength >= length - 0.1f)
            {
                if (flip)
                {
                    transform.Rotate(0, 180, 0);
                    flip = false;
                }
            }
            else
            {
                flip = true;
            }
        }
    }
}