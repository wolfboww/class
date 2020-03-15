using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject[] Maps;
    
    [HideInInspector]
    public Transform revivePoint;
    [HideInInspector]
    public GameObject player;

    private int mapNumber = 0;
    private Transform bulletsList;
    private Transform mask;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        bulletsList = transform.Find("BulletsList");
        mask = player.transform.Find("Mask");
        //ChangeMap();
        //ChangeMap();
        player.transform.position = revivePoint.position;
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject bullet in GameObject.FindGameObjectsWithTag("Bullet"))
        {
            if (bullet.transform.parent != bulletsList)
                bullet.transform.SetParent(bulletsList);
        }
    }

    public void ChangeMap()
    {
        mapNumber++;
        Maps[mapNumber].SetActive(true);
        Maps[mapNumber - 1].SetActive(false);
        for (int i = 0; i < Maps[mapNumber].transform.Find("Boundary").childCount; i++)
            Camera.main.GetComponent<CameraController>().boundary[i]
                = Maps[mapNumber].transform.Find("Boundary").GetChild(i);
        revivePoint = Maps[mapNumber].transform.Find("StartPoint");
    }

    public void Mask(bool be, Sprite sprite)
    {
        player.GetComponent<SpriteRenderer>().enabled = !be;
        player.GetComponent<BoxCollider2D>().enabled = !be;
        player.GetComponent<Rigidbody2D>().gravityScale = be ? 0 : 3;
        player.GetComponent<MoveController>().moveSpeed = be ? 0 : 10;
        mask.GetComponent<SpriteRenderer>().sprite = sprite;

        if (be)
        {
            mask.gameObject.AddComponent<PolygonCollider2D>();
            player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        }
        else
        {
            Destroy(mask.GetComponent<PolygonCollider2D>());
            player.transform.eulerAngles = Vector3.zero;
        }
    }

    public static GameController _instance;
    public static GameController Instance
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
