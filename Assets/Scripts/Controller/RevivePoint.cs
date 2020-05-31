using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevivePoint : MonoBehaviour
{
    public static float edition;
    public GameObject[] EnemyObj;
    public GameObject[] AnimObj;
    public GameObject[] ActiveObj;
    public struct GameObj
    {
        public GameObject prefab;
        public Vector3 pos;
    }
    public struct SetActObj
    {
        public GameObject obj;
        public bool isActive;
    }

    private Animator anim;
    private int life;
    private int mapCollectionNum;
    private float collectionNum;
    private bool revive = false;
    private GameObj[] enemyObj;
    private SetActObj[] setActObj;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        enemyObj = new GameObj[transform.root.Find("Enemy").childCount + EnemyObj.Length];
        for (int i = 0; i < enemyObj.Length; i++)
        {
            if (i < transform.root.Find("Enemy").childCount)
            {
                enemyObj[i].prefab =
                    transform.root.Find("Enemy").GetChild(i).gameObject;
                enemyObj[i].pos = transform.root.Find("Enemy").GetChild(i).position;
            }
            else
            {
                enemyObj[i].prefab = EnemyObj[i - transform.root.Find("Enemy").childCount];
                enemyObj[i].pos = EnemyObj[i - transform.root.Find("Enemy").childCount].transform.position;
            }
        }
        life = CollisionController.life;
        setActObj = new SetActObj[ActiveObj.Length];
        collectionNum = 0;
        for (int i = 0; i < setActObj.Length; i++)
        {
            setActObj[i].obj = ActiveObj[i];
            setActObj[i].isActive = ActiveObj[i].activeInHierarchy;
        }
        if (transform.root.Find("Collection"))
            mapCollectionNum = transform.root.Find("Collection").childCount;
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("Edition", edition);
        if (revive && GameController.isRevive)
        {
            revive = false;
            CollisionController.life = life;
            GameController.deadNum++;
            if (transform.root.Find("Collection"))
                GameController.collectNum = transform.root.Find("Collection").childCount == mapCollectionNum ? collectionNum : collectionNum + mapCollectionNum - transform.root.Find("Collection").childCount;

            StartCoroutine(Revive());
            StartCoroutine(Reset(anim));
            DesCollection();

            if (Mathf.Abs(edition - 1) <= 0.1f)
                return;
            for (int i = 0; i < enemyObj.Length; i++)
            {
                GameObject child = enemyObj[i].prefab;
                if (!child.activeInHierarchy)
                    child.SetActive(true);
                child.transform.position = enemyObj[i].pos;
                if (child.GetComponentInChildren<Animator>())
                {
                    child.GetComponentInChildren<Animator>().SetTrigger("Revive");
                    StartCoroutine(Reset(child.GetComponentInChildren<Animator>()));
                }
            }

            foreach (GameObject item in AnimObj)
            {
                item.GetComponent<Animator>().SetTrigger("Revive");
                StartCoroutine(Reset(item.GetComponent<Animator>()));
            }
            for (int i = 0; i < setActObj.Length; i++)
                setActObj[i].obj.SetActive(setActObj[i].isActive);

            if (GameController.Instance.player.GetComponent<Animator>().GetFloat("Edition") >= 1f)
            {
                if (GameController.Instance.ActiveCam().gameObject != ColliNameManager.Instance.MainCamera)
                    return;
                GameController.Instance.ActiveCam().GetComponent<CameraController>().boundary[1] = transform.root.Find("Boundary").GetChild(1).GetChild(int.Parse(transform.parent.name));
            }
        }
    }

    IEnumerator Revive()
    {
        yield return 1;
        GameController.isRevive = false;
    }

    IEnumerator Reset(Animator anim)
    {
        yield return new WaitForSeconds(0.1f);
        if (anim != null)
            anim.ResetTrigger("Revive");
    }

    void DesCollection()
    {
        GameObject[] obj = FindObjectsOfType(typeof(GameObject)) as GameObject[];
        foreach (GameObject child in obj)
        {
            if (child.tag == "Collection")
            {
                if (child.transform.parent == null)
                    Destroy(child);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GameController.Instance.revivePoint = transform;
            anim.SetTrigger("Revive");
            revive = true;
            if (!GameController.isRevive)
                collectionNum = GameController.collectNum;
            life = life < CollisionController.life ? CollisionController.life : life;
        }
    }
}
