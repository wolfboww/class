using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropControl : MonoBehaviour
{
    private void OnEnable()
    {
        MapManager root = transform.root.GetComponent<MapManager>();
        if (Random.Range(0, 1.0f) <= root.dropRate)
            Instantiate(root.collection, transform.position, Quaternion.identity);
    }
}
