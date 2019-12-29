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

    private int MapNumber = 0;
    private Transform mask;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        mask = player.transform.Find("Mask");
    }

    // Update is called once per frame
    void Update()
    {
        if (Maps[MapNumber].transform.Find("StartPoint") != revivePoint)
            revivePoint = Maps[MapNumber].transform.Find("StartPoint");
    }

    public void ChangeMap()
    {
        MapNumber++;
        Maps[MapNumber].SetActive(true);
        for (int i = 0; i < Maps[MapNumber].transform.Find("Boundary").childCount; i++)
            Camera.main.GetComponent<CameraController>().boundary[i]
                = Maps[MapNumber].transform.Find("Boundary").GetChild(i);
    }

    public void Mask(bool be, Sprite sprite)
    {
        player.GetComponent<SpriteRenderer>().enabled = !be;
        player.GetComponent<BoxCollider2D>().enabled = !be;
        player.GetComponent<Rigidbody2D>().gravityScale = be ? 0 : 3;
        player.GetComponent<MoveController>().moveSpeed = be ? 0 : 10;
        mask.GetComponent<SpriteRenderer>().sprite = sprite;

        if (be)
            mask.gameObject.AddComponent<PolygonCollider2D>();
        else
            Destroy(mask.GetComponent<PolygonCollider2D>());
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
