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
    float i;
    public float age;
    float reprodLap;
    float reprodAge;
    public void Start()
    {
        Hunger = 0;
        age = 0;
        reprodLap = 0;
        reprodAge = Random.Range(35,120);
        body = gameObject.GetComponent<Rigidbody2D>();
        if (isProginator)
        {
            DNA = new Genes(20, 4, 2, 0, new Color(255,255,255));
        }
        if (isPlant)
        {
            DNA.Speed = 0;
        }
        DNA.Speed -= DNA.Resilliance / 2;
        if(DNA.Speed < 0)
        {
            DNA.Speed = 0;
        }
        EnergyUsage = (DNA.Sight * StatModifiers.SightMod) + (DNA.Speed * StatModifiers.SpeedMod) + (DNA.Resilliance*StatModifiers.Resilliance) + (DNA.Damage * StatModifiers.Damage);
        gameObject.GetComponent<SpriteRenderer>().color = DNA.color;
        transform.localScale = new Vector3(DNA.Resilliance/2, DNA.Damage/2 +DNA.Resilliance/2);
        gameObject.GetComponent<Food>().Nutrition = DNA.Resilliance * 20;
        //target = transform.position = Random.insideUnitCircle * 3;
    }
    
    public void Update()
    {
        reprodLap += Time.deltaTime;
        if(reprodLap > reprodAge && Hunger < 100)
        {
            Hunger += 45;
            reprodAge = Random.Range(75, 150);
            reprodLap = 0;
           var child = Instantiate(gameObject, transform.position + QuickMath.RandomVector(-5, 5), Quaternion.identity);
            var ChildGenes = child.GetComponent<Animal>();
            ChildGenes.isProginator = false;
            ChildGenes.DNA.Sight += Random.Range(-2, maxInclusive: 2);
            ChildGenes.DNA.Speed += Random.Range(-2, maxInclusive: 3);
            ChildGenes.DNA.Damage += Random.Range(-2, maxInclusive: 2);
            ChildGenes.DNA.Resilliance += Random.Range(-1, maxInclusive: 1);
            ChildGenes.DNA.color = DNA.color + new Color(Random.Range(-20, maxInclusive: 20), Random.Range(-20, maxInclusive: 20), Random.Range(-20, maxInclusive: 20));
            if(Random.Range(0,101) > 80)
            {
                var AddOrRem = Mathf.Round(Random.Range(0, 2));
                if(AddOrRem == 0 && ChildGenes.diet.Count > 1)
                {
                    ChildGenes.diet.RemoveAt(0);
                }
                else if(AddOrRem == 1)
                {
                    var FoodType = Mathf.Round(Random.Range(0, 2));
                    if(FoodType == 0)
                    {
                        ChildGenes.diet.Add(FoodTypes.Plant);
                    }
                    if(FoodType == 1)
                    {
                        ChildGenes.diet.Add(FoodTypes.Prey);
                    }
                }
            }
            #region Check Zero
            if(ChildGenes.DNA.Sight < 0)
            {
                ChildGenes.DNA.Sight = 0;
            }if(ChildGenes.DNA.Speed < 0)
            {
                ChildGenes.DNA.Speed = 0;
            }if(ChildGenes.DNA.Damage < 0)
            {
                ChildGenes.DNA.Damage = 0;
            }if(ChildGenes.DNA.Resilliance < 0)
            {
                ChildGenes.DNA.Resilliance = 0;
            }
            #endregion


        }

        if (moveState == State.lookingAround)
        {

                i += 1*DNA.Speed;
                transform.Rotate(new Vector3(0, 0, 1*DNA.Speed));
                if (seenEnems)
                {
                   
                    if (diet.Contains(seenEnems.collider.gameObject.GetComponent<Food>().foodType))
                    {
                        if(Random.Range(Hunger,150) > 100)
                        {
                            target = seenEnems.transform.position;
                            moveState = State.MovingIdle;
                        }
                      
                       
                       
                    }
                    if (seenEnems.collider.GetComponent<Animal>())
                    {
                        if (seenEnems.collider.GetComponent<Animal>().diet.Contains(GetComponent<Food>().foodType))
                        {
                            target =  transform.position -transform.up * DNA.Sight;
                            moveState = State.Fleeing;
                        }
                    }
                   
                }

                if (i == 359)
                {

                //moveState = State.MovingIdle;
                    target = transform.position + QuickMath.RandomVector(-10, 10);

                    moveState = State.MovingIdle;


                }
                //if(seenEnems.collider.gameObject)break
            
            
            

        }
       
        if(moveState == State.MovingIdle)
        {
            transform.up = target - transform.position;
            body.velocity = transform.up * DNA.Speed;
            if (Vector2.Distance(transform.position, target) < 0.1f)
            {
                i = 0;
                body.velocity = new Vector3(0, 0);
                moveState = State.lookingAround;
                
            }
            
            
        }
        if (moveState == State.Fleeing)
        {
            transform.up = target - transform.position;
            body.velocity = transform.up * (DNA.Speed*2);
            if (Vector2.Distance(transform.position, target) < 0.1f)
            {
                i = 0;
                body.velocity = new Vector3(0, 0);
                moveState = State.lookingAround;

            }
            Hunger += EnergyUsage * Time.deltaTime;


        }
        age += Time.deltaTime;
        Hunger += EnergyUsage * Time.deltaTime;
        //transform.Rotate(new Vector3(0, 0, 1f));
        seenEnems = Physics2D.Raycast(transform.position + transform.up *transform.localScale.y,transform.up,DNA.Sight);
        if(Hunger > 150)
        {
            Die();
        }
        if(age > 300)
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
         if (diet.Contains(col.gameObject.GetComponent<Food>().foodType) && Hunger > 10 && moveState != State.Fleeing)
         {
            if (col.GetComponent<Animal>())
            {
                var PreyGenes = col.GetComponent<Animal>();
                if(Random.Range(PreyGenes.DNA.Resilliance/2, PreyGenes.DNA.Resilliance) + DNA.Damage > PreyGenes.DNA.Damage)
                {
                    Destroy(col.gameObject);
                    Hunger -= col.gameObject.GetComponent<Food>().Nutrition;
                    if (Hunger < 0)
                    {
                        Hunger = 0;
                    }
                }
            }
            else
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
}
[System.Serializable]
public struct Genes
{
    public float Sight;
    public float Speed;
    public Color color;
    public float Resilliance;
    public float Damage;
    
    public Genes(float s, float sp, float r, float d, Color c)
    {
        this.Sight = s;
        this.Speed = sp;
        this.color = c;
        this.Damage = d;
        this.Resilliance = r;
        
    }

}
public static class StatModifiers
{
    public static readonly float SightMod = 0.15f/2;
    public static readonly float SpeedMod = 1.2f/2;
    public static readonly float Resilliance = 0.7f / 2;
    public static readonly float Damage = 1f / 2;
    
}
public static class QuickMath
{
    public static Vector3 RandomVector(float min, float max)
    {
        return new Vector3(Random.Range(min, max), Random.Range(min, max));
    }
}

