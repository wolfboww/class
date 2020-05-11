﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Continue : MonoBehaviour
{
    private Animator anim;
    private GameObject[] child;

    private bool enter = false;

    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>();
        child = new GameObject[transform.childCount];
        for (int i = 0; i < child.Length; i++)
            child[i] = transform.GetChild(i).gameObject;
    }

    // Update is called once per frame
    void OnEnable()
    {
        StartCoroutine(ContinueImage());
    }

    void Update()
    {
        if (Input.anyKeyDown && child[1].activeInHierarchy)
            enter = true;
    }

    IEnumerator ContinueImage()
    {
        child[0].SetActive(true);
        anim.SetTrigger("IsActive");
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("PleaseEnter"));
        child[1].SetActive(true);
        yield return new WaitUntil(() => enter);
        child[2].SetActive(true);
        anim.SetTrigger("Enter");
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("StartLoading") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);
        child[3].SetActive(true);
    }

    void OnDisable()
    {
        enter = false;
        foreach (var item in child)
            item.SetActive(false);
    }
}