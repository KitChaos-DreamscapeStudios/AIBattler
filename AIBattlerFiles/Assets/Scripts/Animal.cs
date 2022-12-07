using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
    public enum State 
    {
        Fleeing,
        SearchingFood,
        MovingIdle,
        lookingAround,
        Idle
    }
    public Genes DNA;
    public RaycastHit2D seenEnems;
    public State moveState;
    public Vector3 target;
    #region Base Definers
    public bool isProginator;
    public bool isPlant;
    #endregion
    public float Hunger;
    public float EnergyUsage;
    Rigidbody2D body;
    public List<FoodTypes> diet;
    public void Start()
    {
        body = gameObject.GetComponent<Rigidbody2D>();
        if (isProginator)
        {
            DNA = new Genes(10, 5);
        }
        if (isPlant)
        {
            DNA.Speed = 0;
        }
        EnergyUsage = (1 * StatModifiers.SightMod) + (1 * StatModifiers.SpeedMod);
        //target = transform.position = Random.insideUnitCircle * 3;
    }
    IEnumerator SetState(State state, float time = 0)
    {
        //yield return new WaitForSeconds(time);
        moveState = state;
        yield return null;
       
    }
    public void Update()
    {
        if (moveState == State.lookingAround)
        {
            body.velocity = new Vector3(0, 0);
            for(int i =0; i< 360; i++)
            {
                transform.Rotate(new Vector3(0, 0, 1));
                
               
                if (i == 359)
                {
                    target = transform.position + QuickMath.RandomVector(-10, 10);
                    moveState = State.MovingIdle;


                }
                //if(seenEnems.collider.gameObject)break
            }
           
            //SetState(State.MovingIdle);
            

        }
       
        if(moveState == State.MovingIdle)
        {
            transform.up = target - transform.position;
            if (Vector2.Distance(transform.position, target) < 1)
            {

                moveState = State.lookingAround;

            }
            body.velocity = transform.up * DNA.Speed;
            
        }
     
        Hunger += EnergyUsage * Time.deltaTime;
        //transform.Rotate(new Vector3(0, 0, 1f));
        seenEnems = Physics2D.Raycast(transform.up + new Vector3(0,DNA.Sight), new Vector2(DNA.Sight/4, DNA.Sight), transform.rotation.z);
        if(Hunger > 500)
        {
            Die();
        }

    }
    void LookAround()
    {
        

    }
    void Die()
    {
        Destroy(gameObject);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, transform.up*DNA.Sight);
    }
    Transform GetClosestEnemy(Transform[] enemies)
    {
        Transform tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        foreach (Transform t in enemies)
        {
            float dist = Vector3.Distance(t.position, currentPos);
            if (dist < minDist)
            {
                tMin = t;
                minDist = dist;
            }
        }
        return tMin;
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (diet.Contains(col.gameObject.GetComponent<Food>().foodType))
        {
            Destroy(col.gameObject);
            Hunger -= col.gameObject.GetComponent<Food>().Nutrition;
            if (Hunger < 0)
            {
                Hunger = 0;
            }
        }
    }
}
[SerializeField]
public struct Genes
{
    public float Sight;
    public float Speed;
    
    public Genes(float s, float sp)
    {
        this.Sight = s;
        this.Speed = sp;
        
    }

}
public static class StatModifiers
{
    public static readonly float SightMod = 0.15f;
    public static readonly float SpeedMod = 1.2f;
}
public static class QuickMath
{
    public static Vector3 RandomVector(float min, float max)
    {
        return new Vector3(Random.Range(min, max), Random.Range(min, max));
    }
}

