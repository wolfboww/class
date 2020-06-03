using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotMachineManager : MonoBehaviour
{
    public Animator Door;
    private int[] childType;

    // Start is called before the first frame update
    void Start()
    {
        childType = new int[transform.childCount];
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < childType.Length; i++)
            childType[i] = transform.GetChild(i).GetComponent<SlotMachine>().type;

        if (Get())
            Door.SetTrigger("Get");
    }

    private bool Get()
    {
        for (int i = 0; i < childType.Length-1; i++)
        {
            if (childType[i] != childType[i + 1])
                return false;
        }

        return true;
    }
}
