using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class End : MonoBehaviour
{
    void AnimEnd()
    {
        transform.parent.parent.gameObject.SetActive(false);
    }
}
