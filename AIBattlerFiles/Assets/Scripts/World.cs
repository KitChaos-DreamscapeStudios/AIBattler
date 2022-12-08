using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    public static World world;
    public UnityEngine.UI.Image filter;
    public bool IsNight;
    public float elap;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        elap += Time.deltaTime;
        
    }
}
