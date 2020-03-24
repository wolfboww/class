using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopControl : MonoBehaviour
{
    public void PlayerStop(int canMove)
    {
        GameController.Instance.player.GetComponent<MoveController>().canMove 
            = canMove.Equals(0) ? true : false;
    }
}
