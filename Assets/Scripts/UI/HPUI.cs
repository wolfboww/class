using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPUI : MonoBehaviour
{
    public static float edition;
    public static int getHPCollectNum;
    public Sprite[] collection;

    private Transform HP;
    private Animator anim;
    private GameObject[] HPchild;
    private int activeNum;
    private Transform collectionFX;
    private Text collectionText;

    // Start is called before the first frame update
    void Awake()
    {
        edition = 0;
        getHPCollectNum = 4;
        anim = GetComponent<Animator>();
        HP = transform.Find("HP");
        HPchild = new GameObject[HP.childCount];
        for (int i = 0; i < HPchild.Length; i++)
            HPchild[i] = HP.GetChild(i).gameObject;
        collectionFX = transform.Find("Collection");
        collectionText = collectionFX.GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("Edition", edition);
        collectionFX.Find("FX").GetComponent<Image>().sprite = collection[
            (int)RevivePoint.edition >= collection.Length ? collection.Length - 1 :
            (int)RevivePoint.edition];
        collectionText.text = GameController.collectNum.ToString();
        if (GameController.collectNum >= getHPCollectNum
            && HPchild[activeNum].activeInHierarchy)
        {
            GameController.collectNum -= getHPCollectNum;
            CollisionController.life++;
        }

        foreach (Transform item in HP)
        {
            if (item.gameObject.activeInHierarchy)
                item.GetComponent<Animator>().SetFloat("Edition", edition);
        }

        activeAccount();
        Debug.Log(activeNum+"  "+ CollisionController.life);
    }

    void activeAccount()
    {
        activeNum = 0;
        for (int i = 0; i < HPchild.Length; i++)
        {
            if (!HPchild[i].activeInHierarchy)
                continue;
            if (HPchild[i].GetComponent<Image>().enabled)
                activeNum++;
        }

        if (activeNum == CollisionController.life)
            return;
        if (activeNum > CollisionController.life)
        {
            if (activeNum > 0)
                HPchild[activeNum - 1].GetComponent<Animator>().SetTrigger("LoseHP");
        }
        else
            HPchild[activeNum].GetComponent<Image>().enabled = true;
    }
}
