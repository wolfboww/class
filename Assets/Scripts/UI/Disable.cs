using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Disable : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
        StartCoroutine(SetActive());
    }

    IEnumerator SetActive()
    {
        GetComponent<Image>().enabled = false;
        GetComponent<Animator>().ResetTrigger("LoseHP");
        yield return 1;
        this.enabled = false;
    }
}
