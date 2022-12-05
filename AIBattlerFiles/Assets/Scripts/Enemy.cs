using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Rigidbody2D body;
    public float horizontal;
    public float jumpPower = 5;
    public bool isOnGround;
    public LayerMask ground;
    public float moveSpeed = 5;
  
    public GameObject SlashObject;
    public float HealthPoints;
    public bool RemoveControl;
    public List<bool> BadEvents;
    public List<bool> GoodEvents;
    public List<bool> NeutralEvents;
    public float lookDirect;
    float elap;
    float waitTime;
    float MaxWaitTime = 3;
    float MinWaitTime = 0;
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
        if(elap > waitTime)
        {
           
        }

        if (horizontal != 0)
        {
            lookDirect = horizontal;
        }

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
        SetWaittime();
    }
    void Jump()
    {
        body.velocity = new Vector2(body.velocity.x, 1 * jumpPower);
        SetWaittime();
    }
    void SetMovement()
    {
        horizontal = (float)RandChoice(new List<object> { 0, 1, -1 });
        SetWaittime();
    }
    void SetWaittime()
    {
        elap = 0;
        waitTime = Random.Range(MinWaitTime, MaxWaitTime);
    }
    object RandChoice(List<object> Choices)
    {
        return Choices[Random.Range(0, Choices.Count)];
    }
}
