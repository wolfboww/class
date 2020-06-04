using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerController : MonoBehaviour
{
    public GameObject target;
    public GameObject activeCam;

    public bool trigger;

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag.Contains("Player"))
        {
            if (target != null)
                target.GetComponent<PolygonCollider2D>().enabled = trigger;

            GameController.Instance.ActiveCam().gameObject.SetActive(false);
            activeCam.SetActive(true);
        }
    }
}
