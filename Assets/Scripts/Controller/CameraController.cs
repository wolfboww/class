using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float Speed;
    public Transform[] boundary;

    private GameObject player;
    private Vector3 limitPos;
    private float height;
    private float width;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        height = GetComponent<Camera>().orthographicSize;
        width = height * GetComponent<Camera>().aspect;
    }

    // Update is called once per frame
    void Update()
    {
        CameraFollow();
    }

    private void CameraFollow()
    {
        if (Vector2.Distance(player.transform.position, transform.position) > 3)
        {
            if (Mathf.Abs(player.transform.position.y - transform.position.y) > 0.5f)
                transform.position = Vector2.Lerp(transform.position, player.transform.position, Speed * Time.deltaTime);
            else
            {
                float x = Mathf.Lerp(transform.position.x, player.transform.position.x, Speed * Time.deltaTime);
                transform.position = new Vector2(x, transform.position.y);
            }
        }
        limitPos = new Vector3(Mathf.Clamp(transform.position.x, boundary[0].position.x + width, boundary[1].position.x - width), Mathf.Clamp(transform.position.y, boundary[1].position.y + height, boundary[0].position.y - height), -10);
        transform.position = limitPos;
    }
}
