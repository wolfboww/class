using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    // Start is called before the first frame update
    void Update()
    {
        Sound();
    }

    public void Sound()
    {
        Transform[] father = GetComponentsInChildren<Transform>(true);
        foreach (Transform child in father)
        {
            if (child.GetComponent<AudioSource>())
            {
                if (!GameController.sound)
                    child.GetComponent<AudioSource>().mute = true;
                else
                {
                    if (!child.GetComponent<MapManager>() && !child.GetComponent<AstarPath>())
                    {
                        if (GameController.Instance.InCamera(child.transform))
                            child.GetComponent<AudioSource>().mute = false;
                        else
                            child.GetComponent<AudioSource>().mute = true;
                    }
                }
            }
        }
    }
}
