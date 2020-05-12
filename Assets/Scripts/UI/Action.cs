using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action : MonoBehaviour
{
    public GameObject anim;
    public GameObject end;
    public static bool isOver;

    private GameObject commonUse;
    private GameObject UncommonUse;
    private Transform Icon;

    // Start is called before the first frame update
    void Awake()
    {
        commonUse = transform.Find("CommonUse").gameObject;
        UncommonUse = transform.Find("UncommonUse").gameObject;
        Icon = transform.Find("Icon");
    }

    // Update is called once per frame
    void OnEnable()
    {
        commonUse.SetActive(isOver);
        UncommonUse.SetActive(isOver);
        transform.Find("Tip").gameObject.SetActive(isOver);
        Icon.GetComponent<DragUI>().enabled = isOver;
    }

    void Update()
    {
        if (Vector2.Distance(Icon.position, commonUse.transform.position) < 10)
        {
            transform.Find("Common").gameObject.SetActive(true);
            Icon.GetComponent<RectTransform>().position = transform.Find("Common").Find("Point").position;
            Icon.GetComponent<DragUI>().enabled = false;
        }
        else if (Vector2.Distance(Icon.position, UncommonUse.transform.position) < 10)
            end.SetActive(true);
    }

    public void play()
    {
        anim.SetActive(true);
    }
}
