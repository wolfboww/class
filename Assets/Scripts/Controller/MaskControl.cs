using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskControl : MonoBehaviour
{
    private GameObject hatObj;

    // Start is called before the first frame update
    void Start()
    {
        hatObj = Instantiate(ColliNameManager.Instance.Hat, transform.Find("hatPos"));

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDestroy()
    {
        Destroy(hatObj);
    }
}
