using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Enemy : MonoBehaviour
{
    public static Enemy enem;
    public Rigidbody2D body;
    public float horizontal;
    public float jumpPower = 5;
    public bool isOnGround;
    public LayerMask ground;
    public float moveSpeed = 5;
  
    public GameObject SlashObject;
    public float HealthPoints;
    public bool RemoveControl;
   
    public float lookDirect;
    float elap;
    float waitTime;
    float MaxWaitTime = 1;
    float MinWaitTime = 0;
    public delegate void Func();
    Func cut;
    Func setMoveL;
    Func setMoveR;
    Func setMoveN;
    Func leap;
    public List<Func> EnemyChoicesR;
    public List<Func> EnemyChoicesL;
    public bool IsAttacking;
    bool StartStuff;
    public GameObject player;
    public GameObject HealthBar;
    // Start is called before the first frame update
    //Additional Instructions
    //Make sure the object you attatch this to has a Rigidbody2D component attatched to it, and there is a square below it with the Layer "Ground"
    //Both player and floor should have BoxCollider2D's
    //Make sure "ground" in the object with this script attatched is set to the layer you made "Ground"
    //
    void NotAtk()
    {
        IsAttacking = false;
    }
    void Start()
    {
        enem = this;
        #region Ineffecient Action Settings
        lookDirect = -1;
        body = gameObject.GetComponent<Rigidbody2D>();
        cut = Slash;
        setMoveL = SetMovementL;
        setMoveR = SetMovementR;
        setMoveN = SetMovementN;
        leap = Jump;
        EnemyChoicesR = new List<Func>();
        EnemyChoicesL = new List<Func>();

        Invoke("SetUp", 0.5f);
        #endregion
        
    }
    void SetUp()
    {
        for (int i = 0; i <
           //PlayerPrefs.GetInt("EnemCutWeight");
           StateKeeper.RWeights[Weights.Slash]; i++)
        {

            EnemyChoicesR.Add(cut);
        }
        for (int i = 0; i < //PlayerPrefs.GetInt("EnemJumpWeight");
                            StateKeeper.RWeights[Weights.Jump]; i++)
        {

            EnemyChoicesR.Add(leap);
        }
        for (int i = 0; i < //PlayerPrefs.GetInt("EnemMoveLWeight");
                            StateKeeper.RWeights[Weights.MoveLeft]; i++)
        {
            Debug.Log("Added MoveL");
            EnemyChoicesR.Add(setMoveL);
        }
        for (int i = 0; i < //PlayerPrefs.GetInt("EnemMoveNWeight");
                            StateKeeper.RWeights[Weights.MoveRight]; i++)
        {

            EnemyChoicesR.Add(setMoveR);
        }
        for (int i = 0; i < //PlayerPrefs.GetInt("EnemMoveRWeight");
                            StateKeeper.RWeights[Weights.Stop]; i++)
        {

            EnemyChoicesR.Add(setMoveN);
        }

        for (int i = 0; i <
           //PlayerPrefs.GetInt("EnemCutWeight");
           StateKeeper.LWeights[Weights.Slash]; i++)
        {
           
            EnemyChoicesL.Add(cut);
        }
        for (int i = 0; i < //PlayerPrefs.GetInt("EnemJumpWeight");
                            StateKeeper.LWeights[Weights.Jump]; i++)
        {
          
            EnemyChoicesL.Add(leap);
        }
        for (int i = 0; i < //PlayerPrefs.GetInt("EnemMoveLWeight");
                            StateKeeper.LWeights[Weights.MoveLeft]; i++)
        {
            Debug.Log("Added MoveL");
            EnemyChoicesL.Add(setMoveL);
        }
        for (int i = 0; i < //PlayerPrefs.GetInt("EnemMoveNWeight");
                            StateKeeper.LWeights[Weights.MoveRight]; i++)
        {
           
            EnemyChoicesL.Add(setMoveR);
        }
        for (int i = 0; i < //PlayerPrefs.GetInt("EnemMoveRWeight");
                            StateKeeper.LWeights[Weights.Stop]; i++)
        {
            
            EnemyChoicesL.Add(setMoveN);
        }
        StartStuff = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(StartStuff)
        if(HealthPoints <= 0)
        {
            SceneManager.LoadScene("SampleScene");
        }
        elap += Time.deltaTime;

        Debug.Log(EnemyChoicesL.Count);
        Debug.Log(EnemyChoicesR.Count);
        if (elap > waitTime && transform.position.x < player.transform.position.x)
        {
            EnemyChoicesR[Random.Range(0, EnemyChoicesR.Count)]();
        }
        else if (elap > waitTime)
        {
            EnemyChoicesL[Random.Range(0, EnemyChoicesL.Count)]();
        }

        if (horizontal != 0)
        {
            lookDirect = horizontal;
        }
        HealthBar.transform.localScale = new Vector2(10 * (HealthPoints / 20), 1f);
        isOnGround = Physics2D.OverlapBox(transform.position, new Vector2(transform.localScale.x, transform.localScale.y), 0, ground);
    }
    private void FixedUpdate()
    {
        body.velocity = new Vector3(horizontal * moveSpeed, body.velocity.y);
    }
    void Slash()
    {
        var slash = Instantiate(SlashObject, new Vector3(transform.position.x + lookDirect, transform.position.y), Quaternion.identity);
        slash.GetComponent<Rigidbody2D>().velocity = (new Vector2(50 * lookDirect, 0));
        Destroy(slash, 0.05f);
        IsAttacking = true;
        Invoke("NotAtk", 0.05f);
        SetWaittime();
    }
    void Jump()
    {
        body.velocity = new Vector2(body.velocity.x, 1 * jumpPower);
        SetWaittime();
    }
    void SetMovementR()
    {
        horizontal =1;
        SetWaittime();
    }
    void SetMovementL()
    {
        horizontal = -1;
        SetWaittime();
    }
    void SetMovementN()
    {
        horizontal = 0;
        SetWaittime();
    }
    void SetWaittime()
    {
        elap = 0;
        waitTime = Random.Range(MinWaitTime, MaxWaitTime);
    }
    public static void Learn(bool Positive, Dictionary<Weights, int> ToEdit)
    {
        if (Positive)
        {
            if (enem.body.velocity.y > 0 && ToEdit[Weights.Jump] > 0)
            {
                ToEdit[Weights.Jump] += 5;
            }
            if (enem.IsAttacking && ToEdit[Weights.Slash] > 0)
            {
                ToEdit[Weights.Slash] += 5;
            }
            if (enem.horizontal == 1 && ToEdit[Weights.MoveRight] > 0)
            {
                ToEdit[Weights.MoveRight] += 5;
               

            }
            if (enem.horizontal == -1 && ToEdit[Weights.MoveLeft] > 0)
            {
                ToEdit[Weights.MoveLeft] += 5;
            }
            if (enem.horizontal == 0 && StateKeeper.RWeights[Weights.Stop] > 0)
            {
                ToEdit[Weights.Stop] += 5;
            }

        }
        else
        {
            if (enem.body.velocity.y > 0 && ToEdit[Weights.Jump] > 0)
            {
                ToEdit[Weights.Jump] -= 1;
            }
            if (enem.IsAttacking && ToEdit[Weights.Slash] > 0)
            {
                ToEdit[Weights.Slash] -= 1;
            }
            if (enem.horizontal == 1 && ToEdit[Weights.MoveRight] > 0)
            {
                ToEdit[Weights.MoveRight] -= 1;
                ToEdit[Weights.MoveLeft] += 10;

            }
            if (enem.horizontal == -1 && ToEdit[Weights.MoveLeft] > 0)
            {
                ToEdit[Weights.MoveLeft] -= 1;
                ToEdit[Weights.MoveRight] += 10;
            }
            if (enem.horizontal == 0 && StateKeeper.RWeights[Weights.Stop] > 0)
            {
                ToEdit[Weights.Stop] -= 1;
            }
        }
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "PlayerAtk")
        {
            HealthPoints -= 1;
            if (transform.position.x < player.transform.position.x)
            {
                Learn(false, StateKeeper.RWeights);
            }
            else
            {
                Learn(false, StateKeeper.LWeights);
            }
            PlayerPrefs.Save();
        }
    }
    float RandChoice(List<float> Choices)
    {
        return Choices[Random.Range(0, Choices.Count)];
    }
}
