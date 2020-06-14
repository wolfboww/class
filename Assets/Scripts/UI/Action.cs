using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class Action : MonoBehaviour
{
    public GameObject anim;
    public GameObject end;
    public GameObject actionEnd;
    public AudioClip click;
    public Sprite[] buttonDown;
    public Sprite[] giftWindow;
    public static bool isOver;

    private GameObject giftTip;
    private GameObject commonUse;
    private GameObject UncommonUse;
    private Transform setting;
    private Transform Icon;
    private InputField input;
    private bool giftGet = false;

    // Start is called before the first frame update
    void Awake()
    {
        commonUse = transform.Find("CommonUse").gameObject;
        UncommonUse = transform.Find("UncommonUse").gameObject;
        setting = transform.Find("Setting");
        giftTip = transform.Find("Gift").Find("GiftWindow").gameObject;
        Icon = transform.Find("Icon");
        GameController.music = true;
        GameController.sound = true;
    }

    // Update is called once per frame
    void OnEnable()
    {
        if (isOver)
            StartCoroutine(EndAnim());
        commonUse.SetActive(isOver);
        UncommonUse.SetActive(isOver);
        transform.Find("Tip").gameObject.SetActive(isOver);
        Icon.GetComponent<DragUI>().enabled = isOver;
    }

    void Update()
    {
        //if (Vector2.Distance(Icon.position, commonUse.transform.position) < 10)
        //{
        //    //transform.Find("Common").gameObject.SetActive(true);
        //    Icon.GetComponent<RectTransform>().position = transform.Find("Common").Find("Point").position;
        //    //Icon.GetComponent<DragUI>().enabled = false;
        //    Icon.SetParent(transform.Find("Common"));
        //}
        //else if (Vector2.Distance(Icon.position, UncommonUse.transform.position) < 10)
        //    actionEnd.SetActive(true);

        setting.Find("Music").GetComponent<Image>().sprite = GameController.music ? buttonDown[0] : buttonDown[1];
        setting.Find("Sound").GetComponent<Image>().sprite = GameController.sound ? buttonDown[0] : buttonDown[1];
    }

    IEnumerator EndAnim()
    {
        transform.Find("Front").gameObject.SetActive(true);
        yield return new WaitUntil(() => CollisionController.async.isDone);
        end.SetActive(true);
    }

    public void play()
    {
        anim.SetActive(true);
    }

    public void SettingMusic()
    {
        GameController.music = !GameController.music;
    }
    public void SettingSound()
    {
        GameController.sound = !GameController.sound;
    }

    public void Click()
    {
        GetComponent<AudioSource>().Play();
    }

    public void CommonUse()
    {
        transform.Find("Common").gameObject.SetActive(true);
    }

    public void InitialLife()
    {
        CollisionController.life = giftGet ? 2 : 1;
    }

    public void Gift()
    {
        input = transform.Find("Gift").GetComponentInChildren<InputField>();
        char[] dir = input.text.ToCharArray();
        int index = 0;
        for (int i = 0; i < dir.Length; i++)
        {
            if (i > 3)
                return;
            if (Regex.IsMatch(dir[i].ToString(), @"^[+-]?\d*[.]?\d*$"))
                index += int.Parse(dir[i].ToString());
            else
                index += char.ConvertToUtf32(dir[i].ToString().ToLower(), 0) - 87;
        }
        giftTip.SetActive(true);
        if (index == 70 && !giftGet)
        {
            //获得奖励
            giftTip.GetComponent<Image>().sprite = giftWindow[1];
            giftGet = true;
        }
        else if (index == 0)
        {
            //礼包码空
            giftTip.GetComponent<Image>().sprite = giftWindow[0];
        }
        else if (giftGet)
        {
            //已获得
            giftTip.GetComponent<Image>().sprite = giftWindow[3];
        }
        else
        {
            //错误
            giftTip.GetComponent<Image>().sprite = giftWindow[2];
        }
    }
}
