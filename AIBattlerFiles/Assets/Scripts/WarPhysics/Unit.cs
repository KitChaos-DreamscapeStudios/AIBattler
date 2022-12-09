using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public Rigidbody2D body;
    public GameObject shootObj;
    public float ShootVeloc;
    float horizontal;
   
    public float Speed;
    //public float ShootVelocity;
    public Vector3 MousePos;
    public float firerateLimiter;
    float elap;
    bool OnGround;
    public LayerMask ground;
    public Camera Ranger;
    Vector3 RangePos;
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        elap += Time.deltaTime;
        //if (GodPlayer.player.SelectedUnit == this)
        //{
            MousePos = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0);
            RangePos = new Vector3(Ranger.ScreenToWorldPoint(Input.mousePosition).x, Ranger.ScreenToWorldPoint(Input.mousePosition).y, 0);
            if (Input.GetMouseButton(1))
            {
                Ranger.transform.position = new Vector3(transform.position.x + MousePos.x, transform.position.y + MousePos.y, -10) / 2;
            }
            else
            {
                Ranger.transform.position = Vector3.Lerp(Ranger.transform.position, new Vector3(transform.position.x, transform.position.y, -10), 0.01f);
            }


            horizontal = Input.GetAxisRaw("Horizontal");
            if (horizontal != 0)
            {
                body.AddForce(new Vector3(horizontal * Speed, 0));
            }
            OnGround = Physics2D.OverlapBox(transform.position, new Vector2(1, 0.5f), 0, ground);
            if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && OnGround)
            {
                body.AddForce(new Vector3(0, 500));
            }
            if (Input.GetMouseButtonUp(0) && elap > firerateLimiter)
            {
                elap = 0;
                var bullet = Instantiate(shootObj, transform.position, Quaternion.identity);
                bullet.GetComponent<Rigidbody2D>().velocity = (RangePos - bullet.transform.position).normalized * ShootVeloc;
                Destroy(bullet, 5);
            }

        //}

    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position, new Vector2(1, 0.6f));
    }

}
