using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
    public enum Mode
    {
        MoveTo,
        Wander, 
        Escape,
        
    }
    public List<Neuron> turnNeurons;
    public List<Neuron> forwardNeurons;
    public Mode mode;
    public Vector3 MoveToTarget;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            MoveToTarget = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0);
            TryRotate(transform.up - MoveToTarget);
           
        }
    }
    public void TryRotate(Vector3 target)
    {
        if(transform.rotation.eulerAngles.z - target.z > 5)
        {
            foreach (Neuron n in turnNeurons)
            {
                n.Flap(n.rotAmt);
                n.Flap(-n.rotAmt);
            }
            TryRotate(target);
        }
        else
        {
            TryForward(MoveToTarget);
        }
        







    }
    public void TryForward(Vector3 target)
    {
        foreach (Neuron n in forwardNeurons)
        {
            n.Flap(n.forwardRotAmt);
        }
    }
}

public class Neuron : MonoBehaviour
{
    public List<Neuron> SubNeurons;
    public delegate void Func();

    public float rotAmt;
    public float forwardRotAmt;
    public Rigidbody2D body;
    private void Start()
    {
        body = gameObject.GetComponent<Rigidbody2D>();
    }
    public void Flap(float amt)
    {
        body.MoveRotation(amt);
        foreach(Neuron s in SubNeurons)
        {
            if(amt == rotAmt)
            {
                s.Flap(s.rotAmt);
                s.Flap(-s.rotAmt);
            }
            else
            {
                s.Flap(s.forwardRotAmt);
                s.Flap(-s.forwardRotAmt);
            }
            
        }
    }

    
   
}
