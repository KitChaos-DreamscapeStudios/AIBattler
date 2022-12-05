using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateKeeper : MonoBehaviour
{
    public static PlayerState playerState;
    public PlatformerMovement Player;
    public Enemy enemy;
    public EnemyState enemyState;
    // Start is called before the first frame update
    void Start()
    {
        playerState = new PlayerState();
        enemyState = new EnemyState();
    }

    // Update is called once per frame
    void Update()
    {
        enemyState.DistanceFromPlayer = Vector2.Distance(enemy.transform.position, Player.transform.position);
        if (!Player.isOnGround)
        {
            playerState.isInAir = true;
        }
        else
        {
            playerState.isInAir = false;
        }
        if (!enemy.isOnGround)
        {
            enemyState.isInAir = true;
        }
       
        else
        {
            enemyState.isInAir = false;
        }
        if (Player.horizontal != 0)
        {
            playerState.isMovingControlled = true;
        }
        else if (!Player.RemoveControl)
        {
            playerState.isMovingControlled = false;
        }
    }
    
}
public class PlayerState
{
    public bool isInAir;
    public bool isMovingControlled;
    public bool isMovingUncontrolled;
}
public class EnemyState
{
    public float DistanceFromPlayer;
    public bool isInAir;
    public bool isMovingControlled;
}

