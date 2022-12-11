using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using EZCameraShake;
public class Projectile : MonoBehaviour
{
    public GameObject boom;
    public GameObject parent;
    public float ShakeAmount;
    public GameObject muzzleShot;
    // Start is called before the first frame update
    void Start()
    {
        CameraShaker.Instance.ShakeOnce(ShakeAmount / (85 - Vector2.Distance(transform.position, GodPlayer.player.SelectedUnit.transform.position)), 50, 0.1f * ShakeAmount / 2, 0.1f * ShakeAmount);
        Invoke(nameof(SummonMuzz), 0.02f);
    }
    void SummonMuzz()
    {
        Instantiate(muzzleShot, transform.position, Quaternion.identity);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject != parent)
        {
            CameraShaker.Instance.ShakeOnce(ShakeAmount / (85 - Vector2.Distance(transform.position, GodPlayer.player.SelectedUnit.transform.position)), 50, 0.1f * ShakeAmount / 2, 0.1f * ShakeAmount);

            Destroy(gameObject);
            Instantiate(boom, transform.position, Quaternion.identity);
           
            
        }
    
    }
    
    
}
