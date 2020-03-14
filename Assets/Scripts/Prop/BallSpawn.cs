using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawn : MonoBehaviour
{
    public GameObject ball;
    public Transform spawnPos;

    private GameObject ballObj;

    private void Start()
    {
        ballObj = Instantiate(ball, spawnPos.position, Quaternion.identity);
        ballObj.transform.SetParent(spawnPos.parent);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == ballObj)
        {
            ballObj.GetComponent<DestroyController>().enabled = true;
            ballObj = Instantiate(ball, spawnPos.position, Quaternion.identity);
            ballObj.transform.SetParent(spawnPos.parent);
        }
    }
}
