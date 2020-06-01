using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReviveInstant : MonoBehaviour
{
    public GameObject newPrefab;

    private GameObject oldPrefab;

    // Start is called before the first frame update
    void Start()
    {
        oldPrefab = transform.root.Find(newPrefab.name).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.isRevive)
        {
            Destroy(oldPrefab);
            oldPrefab = Instantiate(newPrefab, transform.root);
        }
    }
}
