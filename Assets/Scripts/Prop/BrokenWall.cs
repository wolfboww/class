using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class BrokenWall : MonoBehaviour
{
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name.Equals("PacmanFX"))
        {
            anim.SetTrigger("Dead");
            StartCoroutine(Pacman());
        }
    }

    IEnumerator Pacman()
    {
        for (int i = 0; i < GhostManager.Instance.enemy.childCount; i++)
            GhostManager.Instance.enemy.GetChild(i).GetComponent<Ghost>().status = Ghost.Status.Die;
        yield return new WaitForSeconds(3);
        transform.root.GetComponent<AudioSource>().enabled = true;
        ColliNameManager.Instance.MapPacMan.GetComponentInChildren<PacMan>().InitialBean();
    }
}
