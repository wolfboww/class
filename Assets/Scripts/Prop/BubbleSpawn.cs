using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleSpawn : MonoBehaviour
{
    public GameObject Bubble;
    public float speed;

    private Vector3 initialPos;
    private float bubbleCD = 0.5f;
    private float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        initialPos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.parent.GetComponent<EnemyPatrol>().isDead)
            return;

        timer += Time.deltaTime;
        if (timer > bubbleCD)
        {
            timer = 0;
            GameObject child = Instantiate(Bubble, transform);
            StartCoroutine(BubbleVel(child));
            Destroy(child, 1);
        }
        Vector3 pos = initialPos;
        pos.x = transform.parent.gameObject.GetComponent<SpriteRenderer>().flipX ? -initialPos.x : initialPos.x;
        transform.localPosition = pos;
    }

    IEnumerator BubbleVel(GameObject child)
    {
        child.GetComponent<Velocity>().dir = transform.localPosition.x > 0 ? Velocity.Dir.Right : Velocity.Dir.Left;
        yield return 1;
        child.GetComponent<Velocity>().enabled = true;

    }
}
