using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public enum Weights
{
    Jump, 
    Slash,
    MoveRight,
    MoveLeft,
    Stop
}
public class StateKeeper : MonoBehaviour
{
    public static Dictionary<Weights, int> RWeights;
    public static Dictionary<Weights, int> LWeights;




    // Start is called before the first frame update
    void Start()
    {
        RWeights = new Dictionary<Weights, int>
        {
            {Weights.Slash,  5},
            {Weights.Jump,  5},
            {Weights.MoveRight,  5},
            {Weights.MoveLeft,  5},
            {Weights.Stop,  5}
        };
        LWeights = new Dictionary<Weights, int>
        {
             {Weights.Slash,  5},
            {Weights.Jump,  5},
            {Weights.MoveRight,  5},
            {Weights.MoveLeft,  5},
            {Weights.Stop,  5}
        };
        DontDestroyOnLoad(this);
        SceneManager.LoadScene("SampleScene");
      
       
    }

    // Update is called once per frame
    void Update()
    {

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
    public bool IsAttacking;
    public bool isInAir;
    public bool isMovingControlled;
}

