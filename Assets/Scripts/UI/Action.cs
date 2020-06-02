using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Action : MonoBehaviour
{
    public GameObject anim;
    public GameObject end;
    public GameObject actionEnd;
    public AudioClip click;
    public Sprite[] buttonDown;
    public static bool isOver;

    private GameObject commonUse;
    private GameObject UncommonUse;
    private Transform setting;
    private Transform Icon;


    // Start is called before the first frame update
    void Awake()
    {
        commonUse = transform.Find("CommonUse").gameObject;
        UncommonUse = transform.Find("UncommonUse").gameObject;
        setting = transform.Find("Setting");
        Icon = transform.Find("Icon");
        isOver = false;
    }

    // Update is called once per frame
    void OnEnable()
    {
        end.SetActive(isOver);
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
            actionEnd.SetActive(true);

        setting.Find("Music").GetComponent<Image>().sprite = GameController.music ? buttonDown[0] : buttonDown[1];
        setting.Find("Sound").GetComponent<Image>().sprite = GameController.sound ? buttonDown[0] : buttonDown[1];
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
}
