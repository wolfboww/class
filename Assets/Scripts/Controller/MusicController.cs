using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public int mapNum;
    public AudioClip nextClip;
    public bool waitForLength;

    private AudioSource map;

    private void OnEnable()
    {
        map = GameController.Instance.Maps[mapNum].GetComponent<AudioSource>();
        if (waitForLength)
            StartCoroutine(ChangeClip());
        else
            StartCoroutine(Disable());
    }

    IEnumerator ChangeClip()
    {
        yield return new WaitForSecondsRealtime(map.clip.length);
        yield return StartCoroutine(Disable());
    }

    IEnumerator Disable()
    {
        if (map.clip != nextClip)
        {
            map.clip = nextClip;
            map.Play();
        }
        yield return 1;
        this.enabled = false;
    }
}
