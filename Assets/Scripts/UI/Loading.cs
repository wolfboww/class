using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loading : MonoBehaviour
{
    public static bool loading;

    private Animator computer;
    private Animator play;
    private float edition;
    private bool music;

    // Start is called before the first frame update
    void Awake()
    {
        loading = false;
        computer = transform.Find("Computer").GetComponent<Animator>();
        play = transform.Find("Play").GetComponent<Animator>();
        music = GameController.music;
    }

    void Update()
    {
        computer.SetFloat("Edition", edition);
        play.SetFloat("Edition", edition);
    }

    void OnEnable()
    {
        StartCoroutine(wait());
    }

    IEnumerator wait()
    {
        Time.timeScale = 0;
        GameController.music = false;
        yield return new WaitForSecondsRealtime(3f);
        loading = true;
        yield return 1;
        Time.timeScale = 1;
        GameController.music = music;
        yield return 1;
        gameObject.SetActive(false);
    }


    void OnDisable()
    {
        edition++;
    }
}
