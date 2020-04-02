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

    private GameObject maskObj;
    private Transform maskParent;

    public void Awake()
    {
        _instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Start is called before the first frame update
    void Start()
    {
        bulletsList = transform.Find("BulletsList");
        //mask = player.transform.Find("Mask");
        //ChangeMap();
        //ChangeMap();
        //ChangeMap();
        player.transform.position = revivePoint.position;
    }

    // Update is called once per frame
    void Update()
    {
        ObjAudio();
        foreach (GameObject bullet in GameObject.FindGameObjectsWithTag("Bullet"))
        {
            if (bullet.transform.parent != bulletsList)
                bullet.transform.SetParent(bulletsList);
        }

        if (mapNumber == 3)
            player.GetComponent<Animator>().SetFloat("Edition", 1);

        if (Input.GetKeyDown(KeyCode.Alpha1))
            ColliNameManager.Instance.Gun.transform.position = player.transform.position;
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChangeMap();
            player.transform.position = revivePoint.position;
        }

    }

    public void ChangeMap()
    {
        mapNumber++;
        Maps[mapNumber].SetActive(true);
        Maps[mapNumber - 1].SetActive(false);
        for (int i = 0; i < Maps[mapNumber].transform.Find("Boundary").childCount; i++)
            ActiveCam().GetComponent<CameraController>().boundary[i]
                = Maps[mapNumber].transform.Find("Boundary").GetChild(i);
        revivePoint = Maps[mapNumber].transform.Find("StartPoint");
    }

    public void Mask(GameObject mask)
    {
        if (mask != null)
        {
            maskObj = mask;
            maskParent = mask.transform.parent;
            mask.AddComponent<MaskControl>();
        }
        else
        {
            Destroy(maskObj.GetComponent<MaskControl>());
            maskObj.transform.SetParent(maskParent);
        }

        player.GetComponentInChildren<SpriteRenderer>().enabled = mask == null ? true : false;
        player.transform.position = mask == null ?
            maskObj.transform.position : mask.transform.position;
        if (ActiveCam().GetComponent<CameraController>())
            ActiveCam().GetComponent<CameraController>().follower = mask == null ? player : mask;
    }

    public void ObjAudio()
    {
        GameObject[] obj = FindObjectsOfType(typeof(GameObject)) as GameObject[];
        foreach (GameObject child in obj)
        {
            if (child.GetComponent<AudioSource>())
            {
                float height = ActiveCam().orthographicSize;
                float width = height * ActiveCam().aspect;
                if (child.transform.position.x > ActiveCam().transform.position.x - width && child.transform.position.x < ActiveCam().transform.position.x + width && child.transform.position.y > ActiveCam().transform.position.y - height && child.transform.position.y < ActiveCam().transform.position.y + height)
                    child.GetComponent<AudioSource>().mute = false;
                else
                    child.GetComponent<AudioSource>().mute = true;
            }
        }
    }

    private Camera ActiveCam()
    {
        Camera[] camera = FindObjectsOfType<Camera>();
        foreach (var item in camera)
        {
            if (item.gameObject.activeInHierarchy)
                return item;
        }
        return ColliNameManager.Instance.MainCamera;
    }

    public static GameController _instance;
    public static GameController Instance
    {
        get
        {
            return _instance;
        }
    }

}
