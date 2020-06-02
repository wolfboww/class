using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerController : MonoBehaviour
{
    public GameObject target;
    public bool trigger;

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag.Contains("Player"))
        {
            target.GetComponent<PolygonCollider2D>().enabled = trigger;

            ColliNameManager.Instance.MainCamera.gameObject.SetActive(true);
            ColliNameManager.Instance.FifthCamera.gameObject.SetActive(false);
        }
    }
}
