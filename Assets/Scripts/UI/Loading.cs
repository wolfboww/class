﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loading : MonoBehaviour
{
    public static bool loading;

    private Animator computer;
    private Animator play;
    private float edition;

    // Start is called before the first frame update
    void Awake()
    {
        loading = false;
        computer = transform.Find("Computer").GetComponent<Animator>();
        play = transform.Find("Play").GetComponent<Animator>();
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
        yield return new WaitForSecondsRealtime(3f);
        loading = true;
        gameObject.SetActive(false);
    }


    void OnDisable()
    {
        edition++;
    }
}
