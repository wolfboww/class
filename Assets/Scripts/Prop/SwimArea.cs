using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwimArea : MonoBehaviour
{
    private GameObject playerBubble;

    void Start()
    {
        playerBubble = GameController.Instance.player.transform.Find("BubblePos").gameObject;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            playerBubble.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            playerBubble.SetActive(false);
    }
}
