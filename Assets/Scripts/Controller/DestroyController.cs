using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyController : MonoBehaviour
{
    public float desTime;
    public GameObject prefabpartical;
    void Start()
    {
        if (prefabpartical)
            Instantiate(prefabpartical, gameObject.transform.position, gameObject.transform.rotation);
        Destroy(gameObject, desTime);
    }
}
