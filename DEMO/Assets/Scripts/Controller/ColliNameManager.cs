using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliNameManager : MonoBehaviour
{
    //GameObject
    public GameObject MapPacMan;

    //收集品
    public GameObject Art;

    //Button
    public GameObject DesTrapButton;
    public GameObject RotateButton;

    /// ////////////////////
    public static ColliNameManager _instance;
    public static ColliNameManager Instance
    {
        get
        {
            return _instance;
        }
    }

    public void Awake()
    {
        _instance = this;
    }
}
