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
        height = GetComponent<Camera>().orthographicSize * 2f;
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
            transform.position = Vector2.Lerp(transform.position, player.transform.position, Speed * Time.deltaTime);
        limitPos = new Vector3(Mathf.Clamp(transform.position.x, boundary[0].position.x + width / 2, boundary[1].position.x - width / 2), Mathf.Clamp(transform.position.y, boundary[1].position.y + height / 2, boundary[0].position.y - height / 2), -10);
        transform.position = limitPos;
    }
}
