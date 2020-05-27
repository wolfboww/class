using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseUI : MonoBehaviour
{
    private Animator anim;
    private float timer = 0;
    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (anim.GetFloat("Edition"))
        {
            case 0:
                if (Input.GetMouseButtonDown(0))
                    Continue();
                break;
            case 1:
                if (!Input.GetAxis("Mouse ScrollWheel").Equals(0))
                    Continue();
                break;
            case 2:
                if (Input.GetMouseButtonDown(1))
                    Continue();
                break;
        }

        timer += Time.deltaTime;
        if (timer > 3)
            Continue();

    }

    void Continue()
    {
        GetComponent<StopControl>().PlayerStop(0);
        gameObject.SetActive(false);
    }

    void OnEnable()
    {
        GetComponent<StopControl>().PlayerStop(1);
    }
}
