using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            transform.localScale = new Vector3(1, 1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            transform.localScale = new Vector3(10, 1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            transform.localScale = new Vector3(1, 10);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            transform.localScale = new Vector3(3, 3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            transform.localScale = new Vector3(1, 20);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            transform.localScale = new Vector3(20, 1);
        }
        gameObject.GetComponent<Rigidbody2D>().velocity = (new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0) - transform.position)*5;
    }
}
