using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GlobalStateSaver : MonoBehaviour
{
    //public static PlayerState playerState;
    public PlatformerMovement Player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!Player.isOnGround)
        {
            //playerState.isInAir = true;
        }
        else
        {
            //playerState.isInAir = false;
        }
        if(Player.horizontal != 0)
        {
            //playerState.isMovingControlled = true;
        }
        else if (!Player.RemoveControl)
        {
            //playerState.isMovingControlled = false;
        }
    }
}
//public class PlayerState
//{
//    public bool isInAir;
//    public bool isMovingControlled;
//    public bool isMovingUncontrolled;
//}

