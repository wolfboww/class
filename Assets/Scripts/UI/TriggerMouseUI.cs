using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerMouseUI : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Contains("Player"))
        {
            ColliNameManager.Instance.Mouse.SetActive(true);
            ColliNameManager.Instance.Mouse.GetComponent<Animator>().SetFloat("Edition", 2);
            gameObject.SetActive(false);
        }
    }
}
