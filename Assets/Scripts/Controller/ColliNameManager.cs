using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliNameManager : MonoBehaviour
{
    [Header("GameObject")]
    public GameObject Gun;
    public GameObject Art;
    public GameObject Hat;
    public GameObject MapPacMan;
    public GameObject HandleLand;
    public GameObject[] AnimBoundary;

    [Header("Button")]
    public GameObject DesTrapButton;
    public GameObject RotateButton;

    [Header("Bullet")]
    public GameObject ElseBullet;
    public GameObject IfBullet;

    [Header("Camera")]
    public Camera MainCamera;
    public Camera SecondCamera;
    public Camera ThirdCamera;

    [Header("Audio")]
    public AudioClip enemy1;


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
