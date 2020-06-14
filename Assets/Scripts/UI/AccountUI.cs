using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AccountUI : MonoBehaviour
{
    public Sprite[] collection;
    public static bool go;

    public Image collectionFX;
    public Text[] mapNum;
    public Text collectionText;
    public Text deadText;
    public Text timeText;
    public Animator gridFX;
    public GameObject keepTrying;
    public GameObject getHP;
    public GameObject TheEnd;

    private Animator playerHP;
    private Animator playerImage;
    private Transform bg1;
    private Transform bg2;
    private bool getGrid = false;
    private int accountNum = 0;
    private int awardDeadNum = 6;
    private int awardTime = 600;
    private int mapLife;


    // Start is called before the first frame update
    void Awake()
    {
        playerHP = transform.parent.Find("PlayerHP").GetComponent<Animator>();
        playerImage = transform.Find("PlayerImage").GetComponent<Animator>();
        bg1 = transform.Find("BG");
        bg2 = transform.Find("BG2");
        mapLife = CollisionController.life;
        GameController.Instance.MuteControl(gameObject);
    }

    private void OnEnable()
    {
        go = false;
        Time.timeScale = 0;
        StartCoroutine(mapAccount());
    }

    private void OnDisable()
    {
        Time.timeScale = 1;
        accountNum++;

        if (getGrid)
        {
            deadText.rectTransform.localScale = Vector3.one * 1.5f;
            timeText.rectTransform.localScale = Vector3.one * 2f;
            getGrid = false;
        }
        GameController.deadNum = 0;
        GameController.timeNum = 0;
        GameController.collectNum = 0;
        deadText.GetComponent<Outline>().enabled = false;
        timeText.GetComponent<Outline>().enabled = false;
        for (int i = 0; i < bg1.childCount; i++)
            bg1.GetChild(i).gameObject.SetActive(false);
        for (int i = 0; i < bg2.childCount; i++)
            bg2.GetChild(i).gameObject.SetActive(false);
        bg1.GetComponent<Animator>().ResetTrigger("Account");
        bg2.GetComponent<Animator>().ResetTrigger("Account");
    }

    // Update is called once per frame
    void Update()
    {
        accountNum = accountNum > collection.Length ? collection.Length : accountNum;
        collectionFX.sprite = collection[accountNum];
        gridFX.SetFloat("grid", accountNum);
        playerImage.SetFloat("Edition", accountNum);

        foreach (var item in mapNum)
        {
            string text = "";
            if (accountNum.Equals(0))
                text = "Beginning";
            else if (accountNum.Equals(1))
                text = "TourofFC";
            else if (accountNum.Equals(2))
                text = "GBinPalm";
            item.text = text;
        }
    }

    IEnumerator mapAccount()
    {
        bg1.GetComponent<Animator>().SetTrigger("Account");
        yield return new WaitUntil(() => bg1.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Account"));
        yield return new WaitUntil(() => bg1.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);
        deadText.text = GameController.deadNum.ToString();
        collectionText.text = GameController.collectAccountNum.ToString();
        timeText.text = TimeAccount();
        yield return 1;
        foreach (Transform item in bg1)
            item.gameObject.SetActive(true);
        if (!accountNum.Equals(2))
        {
            if (GameController.deadNum < awardDeadNum)
            {
                Tweener tweener = deadText.rectTransform.DOScale(Vector3.one * 2.5f, 2);
                tweener.SetUpdate(true);
                deadText.GetComponent<Outline>().enabled = true;
                getGrid = true;
            }
            if (GameController.timeNum < awardTime)
            {
                Tweener tweener = timeText.rectTransform.DOScale(Vector3.one * 2.5f, 2);
                tweener.SetUpdate(true);
                timeText.GetComponent<Outline>().enabled = true;
                getGrid = true;
            }
            yield return new WaitForSecondsRealtime(2f);
        }
        bg2.GetComponent<Animator>().SetTrigger("Account");
        yield return new WaitUntil(() => bg2.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Account"));
        yield return new WaitUntil(() => bg2.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);
        bg2.Find("ING").gameObject.SetActive(true);
        bg2.Find("Continue").gameObject.SetActive(true);
        if (getGrid)
        {
            getHP.SetActive(true);
            mapLife = mapLife < 3 ? mapLife + 1 : 3;
        }
        else if (!accountNum.Equals(2))
            keepTrying.SetActive(true);
        else
            TheEnd.SetActive(true);
    }

    private string TimeAccount()
    {
        int Min = (int)GameController.timeNum / 60;
        int Second = (int)GameController.timeNum - Min * 60;

        string min = Min < 10 ? "0" + Min.ToString() : Min.ToString();
        string second = Second < 10 ? "0" + Second.ToString() : Second.ToString();
        return min + ":" + second;
    }

    public void Continue()
    {
        go = true;
        HPUI.getHPCollectNum += 2;
        CollisionController.life = mapLife;
        GameController.Instance.player.GetComponent<AudioSource>().clip = ColliNameManager.Instance.click;
        GameController.Instance.player.GetComponent<AudioSource>().Play();
        ColliNameManager.Instance.account.SetActive(false);
    }
}
