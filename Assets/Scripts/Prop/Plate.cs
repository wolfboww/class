using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour
{
    private Animator anim;
    private bool isOpen = true;

    private float timer = 0;
    private float closeTime = 10;
    private float openTime = 2;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (isOpen)
        {
            if (timer >= openTime)
            {
                anim.SetBool("IsDown", true);
                timer = 0;
            }
        }
        else
        {
            if (timer >= closeTime)
            {
                anim.SetBool("IsDown", false);
                timer = 0;
            }
        }
    }

    public void IsOpen(int be)
    {
        isOpen = be == 0 ? true : false;
    }
}
