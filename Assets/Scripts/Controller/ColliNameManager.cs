using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliNameManager : MonoBehaviour
{
    [Header("GameObject")]
    public GameObject Follower;
    public GameObject Gun;
    public GameObject Art;
    public GameObject Hat;
    public GameObject MapPacMan;
    public GameObject HandleLand;
    public GameObject SwimArea;
    public GameObject Princess;
    public GameObject BossSkate;
    public GameObject BossWinner;
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
    public Camera ForthCamera;

    [Header("Audio")]
    public AudioClip enemy1;

    [Header("UI")]
    public GameObject account;

    [Header("Enemy")]
    public GameObject Enemy1;
    public GameObject Enemy2;
    public GameObject Enemy3;
    public GameObject Enemy4;
    public GameObject Enemy5;



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
