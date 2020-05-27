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

    private Animator playerHP;
    private Animator playerImage;
    private Transform bg1;
    private Transform bg2;
    private bool getGrid = false;
    private int accountNum = 0;
    private int awardDeadNum = 6;
    private int awardTime = 600;


    // Start is called before the first frame update
    void Awake()
    {
        playerHP = transform.parent.Find("PlayerHP").GetComponent<Animator>();
        playerImage = transform.Find("PlayerImage").GetComponent<Animator>();
        bg1 = transform.Find("BG");
        bg2 = transform.Find("BG2");
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
        getGrid = false;
        GameController.deadNum = 0;
        GameController.timeNum = 0;
        GameController.collectNum = 0;
        deadText.GetComponent<Outline>().enabled = false;
        timeText.GetComponent<Outline>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        collectionFX.sprite = collection[accountNum];
        gridFX.SetFloat("grid", accountNum);
        playerImage.SetFloat("Edition", accountNum);
        foreach (var item in mapNum)
            item.text = (accountNum + 1).ToString();
    }

    IEnumerator mapAccount()
    {
        yield return new WaitUntil(() => bg1.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);
        deadText.text = GameController.deadNum.ToString();
        collectionText.text = GameController.collectAccountNum.ToString();
        timeText.text = TimeAccount();
        yield return 1;
        foreach (Transform item in bg1)
            item.gameObject.SetActive(true);
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
        bg2.GetComponent<Animator>().updateMode = AnimatorUpdateMode.UnscaledTime;
        yield return new WaitUntil(() => bg2.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);
        bg2.Find("ING").gameObject.SetActive(true);
        bg2.Find("Continue").gameObject.SetActive(true);
        if (getGrid)
        {
            getHP.SetActive(true);
            playerHP.SetFloat("grid", playerHP.GetFloat("grid") + 1);
        }
        else
            keepTrying.SetActive(true);

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
        ColliNameManager.Instance.account.SetActive(false);
    }
}
