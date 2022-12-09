using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Season
{
    Spring,
    Summer,
    Fall,
    Winter
}
public class World : MonoBehaviour
{
    public static World world;
    public UnityEngine.UI.Image filter;
    public bool IsNight;
    public float elap;
    public Color NightColor;
    public float NightMod;
    public 
    // Start is called before the first frame update
    void Start()
    {
        world = this;   
    }

    // Update is called once per frame
    void Update()
    {
        if (IsNight)
        {
            NightMod = 5;
        }
        else
        {
            NightMod = 0;
        }
        elap += Time.deltaTime;
        if(elap > 60)
        {
            IsNight = true;
            filter.color = NightColor;
        }
        else
        {
            IsNight = false;
            filter.color = new Color(0, 0, 0, 0);
        }
        if(elap > 120)
        {
            elap = 0;
        }
        
    }
}
