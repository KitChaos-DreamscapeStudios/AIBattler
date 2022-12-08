using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CamMovement : MonoBehaviour
{
    public TMPro.TextMeshProUGUI timeScale;
    float horizontal;
    float vertical;
    bool isPaused;
    
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 2;
    }

    // Update is called once per frame
    void Update()
    {
        timeScale.text = $"Time Speed: {Time.timeScale}";
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        transform.position += new Vector3(horizontal / 4, vertical / 4);
        Camera.main.orthographicSize -= Input.mouseScrollDelta.y;
        if(Camera.main.orthographicSize < 5)
        {
            Camera.main.orthographicSize = 5;
        }
        if (Input.GetKeyUp(KeyCode.Equals))
        {
            Time.timeScale += 1;
        }
        if (Input.GetKeyUp(KeyCode.Minus) && Time.timeScale > 0)
        {
            Time.timeScale -= 1;
        }
        if (Input.GetKeyUp(KeyCode.Space) && !isPaused)
        {
            isPaused = true;
            Time.timeScale = 0;
        }
        else if (Input.GetKeyUp(KeyCode.Space) && isPaused)
        {
            isPaused = false;
            Time.timeScale = 1;
        }
    }
}
