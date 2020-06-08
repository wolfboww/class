using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject[] Maps;
    public static bool isBoss;
    public static bool isRevive;
    public static bool music;
    public static bool sound;

    public static float deadNum;
    public static float collectNum;
    public static float collectAccountNum;
    public static float timeNum;

    [HideInInspector]
    public Transform revivePoint;
    public Transform reviveBossPoint;
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
        CollisionController.life = 1;
        RevivePoint.edition = 0;
        timeNum = 0;
        deadNum = 0;
        collectNum = 0;
        collectAccountNum = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        bulletsList = transform.Find("BulletsList");
        player.transform.position = revivePoint.position;
        ColliNameManager.Instance.Loading.SetActive(true);
        isBoss = false;
        isRevive = false;
    }

    // Update is called once per frame
    void Update()
    {
        ObjAudio();
        EnemyActive();
        timeNum += Time.deltaTime;
        foreach (GameObject bullet in GameObject.FindGameObjectsWithTag("Bullet"))
        {
            if (bullet.transform.parent != bulletsList)
                bullet.transform.SetParent(bulletsList);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ColliNameManager.Instance.Gun.transform.position = player.transform.position;
            ColliNameManager.Instance.Art.SetActive(true);
            ColliNameManager.Instance.Art.transform.position = player.transform.position;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChangeMap();
            player.transform.position = revivePoint.position;
            //player.GetComponent<Animator>().SetFloat("Edition", player.GetComponent<Animator>().GetFloat("Edition") + 1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (CollisionController.life < 3)
                CollisionController.life++;
        }

        for (int i = 0; i < Maps.Length; i++)
            if (Maps[i].GetComponent<AudioSource>())
                Maps[i].GetComponent<AudioSource>().mute = !music;

        if (ActiveCam() == ColliNameManager.Instance.MainCamera && !ColliNameManager.Instance.MainCamera.gameObject.activeInHierarchy)
            ColliNameManager.Instance.MainCamera.gameObject.SetActive(true);

        if (mapNumber == 4 && isRevive)
            ColliNameManager.Instance.MainCamera.GetComponent<MusicController>().enabled = true;
    }

    public void ChangeMap()
    {
        mapNumber++;
        HPUI.edition++;
        RevivePoint.edition++;
        Maps[mapNumber].SetActive(true);
        Maps[mapNumber - 1].SetActive(false);
        for (int i = 0; i < Maps[mapNumber].transform.Find("Boundary").childCount; i++)
            ActiveCam().GetComponent<CameraController>().boundary[i]
                = Maps[mapNumber].transform.Find("Boundary").GetChild(i);
        revivePoint = Maps[mapNumber].transform.Find("StartPoint").GetChild(0);
        ActiveCam().transform.position = revivePoint.position;
    }

    public void Mask(GameObject mask)
    {
        if (mask != null)
        {
            maskObj = mask;
            maskParent = mask.transform.parent;
            if (!mask.GetComponent<MaskControl>())
                mask.AddComponent<MaskControl>();
        }
        else
        {
            Destroy(maskObj.GetComponent<MaskControl>());
            maskObj.transform.SetParent(maskParent);
        }

        player.GetComponentInChildren<SpriteRenderer>().enabled = mask == null ? true : false;
        player.transform.position = mask == null ?
            maskObj.transform.Find("hatPos").position : mask.transform.position;
        if (ActiveCam().GetComponent<CameraController>())
            ActiveCam().GetComponent<CameraController>().follower =
                mask == null ? ColliNameManager.Instance.Follower : mask;
    }

    public void ObjAudio()
    {
        GameObject[] obj = FindObjectsOfType(typeof(GameObject)) as GameObject[];
        foreach (GameObject child in obj)
        {
            if (child.GetComponent<AudioSource>())
            {
                if (!sound)
                {
                    child.GetComponent<AudioSource>().mute = true;
                    return;
                }

                if (InCamera(child.transform))
                    child.GetComponent<AudioSource>().mute = false;
                else
                    child.GetComponent<AudioSource>().mute = true;
            }
        }
    }

    public void EnemyActive()
    {
        Transform enemy = Maps[mapNumber].transform.Find("Enemy");
        foreach (Transform child in enemy)
        {
            if (InCamera(child) && child.name != "ActiveEnemy")
                child.gameObject.SetActive(true);
        }
    }

    public Camera ActiveCam()
    {
        Camera[] camera = FindObjectsOfType<Camera>();
        foreach (var item in camera)
        {
            if (item.gameObject.activeInHierarchy)
                return item;
        }
        return ColliNameManager.Instance.MainCamera;
    }

    private bool InCamera(Transform child)
    {
        float height = ActiveCam().orthographicSize;
        float width = height * ActiveCam().aspect;

        if (child.position.x > ActiveCam().transform.position.x - width
            && child.position.x < ActiveCam().transform.position.x + width
            && child.position.y > ActiveCam().transform.position.y - height
            && child.position.y < ActiveCam().transform.position.y + height)
            return true;
        else
            return false;
    }

    public void BulletLookAt(Transform obj, Vector3 target)
    {
        Vector2 direction = target - obj.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        obj.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public IEnumerator ResetAnim(Animator anim, string name)
    {
        anim.SetTrigger(name);
        yield return 2;
        anim.ResetTrigger(name);
    }

    public IEnumerator Language(Transform target, string text1, string text2)
    {
        string text = Random.Range(0, 1.0f) > 0.5f ? text1 : text2;
        if (target.Find("Language").GetComponentInChildren<Text>().text == text)
            StopCoroutine(Language(target, text1, text2));
        target.Find("Language").GetComponentInChildren<Text>().text = text;
        yield return new WaitForSecondsRealtime(0.5f);
        target.Find("Language").GetComponentInChildren<Text>().text = "";
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
