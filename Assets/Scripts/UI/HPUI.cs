using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPUI : MonoBehaviour
{
    public static float edition;

    private Transform HP;
    private Animator anim;
    private GameObject[] HPchild;
    private int activeNum;


    // Start is called before the first frame update
    void Awake()
    {
        edition = 0;
        anim = GetComponent<Animator>();
        HP = transform.Find("HP");
        HPchild = new GameObject[HP.childCount];
        for (int i = 0; i < HPchild.Length; i++)
            HPchild[i] = HP.GetChild(i).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("Edition", edition);
        foreach (Transform item in HP)
            item.GetComponent<Animator>().SetFloat("Edition", edition);

        if (HPchild[2].GetComponent<Image>().enabled)
            activeNum = 3;
        else if (HPchild[1].GetComponent<Image>().enabled)
            activeNum = 2;
        else if (HPchild[0].GetComponent<Image>().enabled)
            activeNum = 1;
        else
            activeNum = 0;

        if (activeNum == CollisionController.life)
            return;
        if (activeNum > CollisionController.life)
        {
            HPchild[activeNum - 1].GetComponent<Animator>().SetTrigger("LoseHP");
        }
        else
            HPchild[activeNum].GetComponent<Image>().enabled = true;
    }

}
