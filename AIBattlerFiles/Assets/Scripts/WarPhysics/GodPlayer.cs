using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;
public class GodPlayer : MonoBehaviour
{
    public Unit SelectedUnit;
    public static GodPlayer player;
    public List<Unit> UnitsInPlay;
    // Start is called before the first frame update
    void Start()
    {
        player = this;
        UnitsInPlay = new List<Unit>(GameObject.FindObjectsOfType<Unit>());
        SelectedUnit = GameObject.FindObjectOfType<Unit>();
    }

    // Update is called once per frame
    void Update()
    {
        Camera.main.transform.parent = SelectedUnit.transform;
        Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, -10);
       
        if (Input.GetKeyDown(KeyCode.E))
        {
            //Destroy(SelectedUnit.gameObject.GetComponent<CameraShaker>());
            SelectedUnit = UnitsInPlay[UnitsInPlay.IndexOf(SelectedUnit) + 1];
            //SelectedUnit.gameObject.AddComponent<CameraShaker>();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //Destroy(SelectedUnit.gameObject.GetComponent<CameraShaker>());
            SelectedUnit = UnitsInPlay[UnitsInPlay.IndexOf(SelectedUnit) - 1];
            //SelectedUnit.gameObject.GetComponent<CameraShaker>();
        }
    }
}
