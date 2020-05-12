using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnimUI : MonoBehaviour
{
    private Transform middle;
    private Transform end;
    private GameObject enemy;
    private GameObject player;
    private float size;

    private void Awake()
    {
        middle = transform.Find("Middle");
        end = transform.Find("End");
        enemy = transform.Find("Enemy").gameObject;
        player = transform.Find("Player").gameObject;
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        size = Mathf.Abs(middle.position.x - player.transform.position.x) / 268.5629f;
        StartCoroutine(AnimStart());
    }

    IEnumerator AnimStart()
    {
        while (enemy.transform.position.x < middle.position.x)
        {
            enemy.transform.Translate((middle.position - enemy.transform.position).normalized * size);
            yield return 1;
        }
        yield return new WaitUntil(() => Mathf.Abs(enemy.transform.position.x - middle.position.x) < 1f * size);
        enemy.GetComponent<Animator>().SetInteger("Step", 1);
        yield return new WaitForSeconds(enemy.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
        while (player.transform.position.x < middle.position.x - 50 * size)
        {
            player.transform.Translate((middle.position - player.transform.position).normalized * 2 * size);
            yield return 1;
        }
        yield return new WaitUntil(() => Mathf.Abs(player.transform.position.x - middle.position.x) < 50f * size);
        enemy.GetComponent<Animator>().SetInteger("Step", 0);
        yield return 1;
        while (enemy.transform.position.x < end.position.x - 100 * size)
        {
            enemy.transform.Translate((end.position - enemy.transform.position).normalized * size);
            player.transform.Translate((end.position - player.transform.position).normalized * 2 * size);
            yield return 1;
        }
        yield return new WaitUntil(() => Mathf.Abs(player.transform.position.x - end.position.x) < 50f * size);

        enemy.GetComponent<Animator>().SetInteger("Step", 2);
        player.GetComponent<Animator>().SetInteger("Step", 1);
        yield return new WaitForSeconds(enemy.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
        SceneManager.LoadScene("Game");
    }
}
