using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//To Add this script, create a C# file, and name it PlatformerMovement(Exaclty that) and then paste this entire script in, from start to end.
//Alternatively, if you wish to name your file something else, only paste in the content below line 7, and remove the last curlybrace.
public class PlatformerMovement: MonoBehaviour
{
    public Rigidbody2D body;
    public float horizontal;
    public float jumpPower = 5;
    public bool isOnGround;
    public LayerMask ground;
    public float moveSpeed = 5;
    public float SlashCooldown;
    float CurrentCool;
    public GameObject SlashObject;
    public float HealthPoints;
    public bool RemoveControl;
    public float lookDirect;
    public Enemy enemy;
    public GameObject HealthBar;
    float JumpElap;
    // Start is called before the first frame update
    //Additional Instructions
    //Make sure the object you attatch this to has a Rigidbody2D component attatched to it, and there is a square below it with the Layer "Ground"
    //Both player and floor should have BoxCollider2D's
    //Make sure "ground" in the object with this script attatched is set to the layer you made "Ground"
    //
    
    void Start()
    {
        lookDirect = 1;
        body = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        JumpElap += Time.deltaTime;
        if(horizontal != 0)
        {
            lookDirect = horizontal;
        }
        CurrentCool += Time.deltaTime;
        if(Input.GetMouseButtonUp(0) && CurrentCool > SlashCooldown)
        {
            CurrentCool = 0;
            var slash = Instantiate(SlashObject, new Vector3(transform.position.x + lookDirect, transform.position.y), Quaternion.identity);
            slash.GetComponent<Rigidbody2D>().velocity = (new Vector2(50 * lookDirect, 0));
            Destroy(slash, 0.05f);
        }
        horizontal = Input.GetAxisRaw("Horizontal");
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) && JumpElap > 0.5f)
        {
            JumpElap = 0;
            body.velocity = new Vector2(body.velocity.x, 1 * jumpPower);

        }
        isOnGround = Physics2D.OverlapBox(transform.position, new Vector2(transform.localScale.x, transform.localScale.y), 0, ground);
        if(HealthPoints <= 0)
        {
            SceneManager.LoadScene("SampleScene");
        }
        HealthBar.transform.localScale = new Vector2(10 * (HealthPoints / 20), 1f);
    }
    private void FixedUpdate()
    {   
        body.velocity = new Vector3(horizontal * moveSpeed, body.velocity.y);
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Damager")
        {
            HealthPoints -= 1;
            if(enemy.transform.position.x < transform.position.x)
            {
                Enemy.Learn(true, StateKeeper.LWeights);
            }
            else
            {
                Enemy.Learn(true, StateKeeper.RWeights);
            }
          
           
        }
    }
}
