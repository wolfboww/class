using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyfactory : MonoBehaviour
{
    public Transform createpos;
    public GameObject prefabenemy;
    public bool create = true;

    void Update()
    {
        if(create)
        {
            InvokeRepeating("createenemy",2,2);
        }
    }
    void createenemy()
    {
        GameObject enemy = Instantiate(prefabenemy,createpos.position,prefabenemy.transform.rotation);
    }
}