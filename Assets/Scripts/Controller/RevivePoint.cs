using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevivePoint : MonoBehaviour
{
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

    private bool revive = false;
    private GameObj[] enemyObj;
    private SetActObj[] setActObj;

    // Start is called before the first frame update
    void Start()
    {
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
                enemyObj[i].prefab = EnemyObj[i - enemyObj.Length];
                enemyObj[i].pos = EnemyObj[i - enemyObj.Length].transform.position;
            }
        }

        setActObj = new SetActObj[ActiveObj.Length];
        for (int i = 0; i < setActObj.Length; i++)
        {
            setActObj[i].obj = ActiveObj[i];
            setActObj[i].isActive = ActiveObj[i].activeInHierarchy;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (revive && GameController.isRevive)
        {
            revive = false;
            StartCoroutine(Revive());
            for (int i = 0; i < enemyObj.Length; i++)
            {
                GameObject child = enemyObj[i].prefab;
                if (!child.activeInHierarchy)
                    child.SetActive(true);
                child.transform.position = enemyObj[i].pos;
                if (child.GetComponentInChildren<Animator>())
                    child.GetComponentInChildren<Animator>().SetTrigger("Revive");
            }

            foreach (GameObject item in AnimObj)
                item.GetComponent<Animator>().SetTrigger("Revive");
            for (int i = 0; i < setActObj.Length; i++)
                setActObj[i].obj.SetActive(setActObj[i].isActive);

            if (GameController.Instance.player.GetComponent<Animator>().GetFloat("Edition") > 0.1f)
                GameController.Instance.ActiveCam().GetComponent<CameraController>().boundary[1] = transform.root.Find("Boundary").GetChild(1).GetChild(int.Parse(transform.parent.name));
        }
    }

    IEnumerator Revive()
    {
        yield return 1;
        GameController.isRevive = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GameController.Instance.revivePoint = transform;
            revive = true;
        }
    }
}
