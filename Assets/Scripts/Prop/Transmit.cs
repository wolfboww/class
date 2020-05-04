using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transmit : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
        GameController.Instance.player.transform.position = transform.GetChild(0).position;
    }

}
