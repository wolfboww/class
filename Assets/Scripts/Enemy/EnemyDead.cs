using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDead : MonoBehaviour
{
    void Dead()
    {
        GetComponentInChildren<Animator>().ResetTrigger("Dead");
    }
}
