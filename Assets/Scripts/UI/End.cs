using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class End : MonoBehaviour
{
    public bool lighting = false;
    public GameObject action;

    private GameObject player;
    private GameObject playerNow;
    private GameObject enemy;
    private Transform target1;
    private Transform target2;
    private float size;
    // Start is called before the first frame update
    void Awake()
    {
        player = transform.Find("Player").gameObject;
        playerNow = transform.Find("PlayerNow").gameObject;
        enemy = transform.Find("Enemy").gameObject;
        target1 = transform.Find("PlayerTarget");
        target2 = transform.Find("PlayerNowTarget");
    }

    void OnEnable()
    {
        size = Mathf.Abs(target1.position.x - player.transform.position.x) / 268.5629f;
        StartCoroutine(EndUI());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
            lighting = true;
    }


    IEnumerator EndUI()
    {
        yield return new WaitUntil(() => lighting);
        enemy.SetActive(true);
        enemy.GetComponent<Animator>().SetInteger("Step", 1);
        player.GetComponent<Animator>().SetInteger("Step", 1);
        yield return new WaitForSeconds(enemy.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
        while (player.transform.position.x > target1.position.x)
        {
            enemy.transform.Translate(new Vector3((target1.position.x - enemy.transform.position.x), 0, 0).normalized * 2 * size);
            player.transform.Translate((target1.position - player.transform.position).normalized * 2 * size);
            yield return 1;
        }
        yield return new WaitUntil(() => Mathf.Abs(player.transform.position.x - target1.position.x) < 5f * size);
        playerNow.SetActive(true);
        while (playerNow.transform.position.x > target2.position.x)
        {
            playerNow.transform.Translate((target2.position - playerNow.transform.position).normalized * size);
            yield return 1;
        }
        yield return new WaitUntil(() => Mathf.Abs(playerNow.transform.position.x - target2.position.x) < 5f * size);
        playerNow.GetComponent<Animator>().SetInteger("Step", 1);
        yield return 1;
        playerNow.GetComponent<Animator>().SetInteger("Step", 0);
        Action.isOver = true;
        yield return new WaitForSeconds(1);
        action.SetActive(true);
    }
}
