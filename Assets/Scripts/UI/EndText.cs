using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EndText : MonoBehaviour
{
    private Transform people;

    // Start is called before the first frame update
    void Awake()
    {
        people = transform.Find("Text");
    }

    // Update is called once per frame
    void OnEnable()
    {
        people.DOLocalMoveY(1000, 10);
    }
}
