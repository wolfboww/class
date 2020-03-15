using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatAnim : MonoBehaviour
{
    public GameObject[] enemy;

    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var item in enemy)
        {
            if (item.activeInHierarchy)
                return;
        }

        Invoke("Anim", 1);
    }

    private void Anim()
    {
        anim.SetTrigger("Do");

    }
}
