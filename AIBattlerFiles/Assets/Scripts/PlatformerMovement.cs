using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    // Start is called before the first frame update
    //Additional Instructions
    //Make sure the object you attatch this to has a Rigidbody2D component attatched to it, and there is a square below it with the Layer "Ground"
    //Both player and floor should have BoxCollider2D's
    //Make sure "ground" in the object with this script attatched is set to the layer you made "Ground"
    //
    
    void Start()
    {
        body = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        CurrentCool += Time.deltaTime;
        if(Input.GetMouseButtonUp(0) && CurrentCool > SlashCooldown)
        {
            CurrentCool = 0;
            Instantiate(SlashObject, new Vector3(transform.position.x + horizontal / 2, transform.position.y), Quaternion.identity);
        }
        horizontal = Input.GetAxisRaw("Horizontal");
        if (Input.GetKeyDown(KeyCode.UpArrow) && isOnGround == true)
        {
            body.velocity = new Vector2(body.velocity.x, 1 * jumpPower);

        }
        isOnGround = Physics2D.OverlapBox(transform.position, new Vector2(transform.localScale.x, transform.localScale.y), 0, ground);
    }
    private void FixedUpdate()
    {
        body.velocity = new Vector3(horizontal * moveSpeed, body.velocity.y);
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Damager")
        {

        }
    }
}
