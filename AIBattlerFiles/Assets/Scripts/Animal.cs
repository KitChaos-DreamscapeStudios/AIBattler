using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AnimalNames
{
    public static List<char> Letters = new List<char> {'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z'};
    public static string GenName()
    {
        string Name = "";
        for(int i = 0; i < 10; i++)
        {
            Name += Letters[Random.Range(0, Letters.Count)];
        }
        return Name;
    }
}
public class Animal : MonoBehaviour
{
  
    public TMPro.TextMeshProUGUI AnimalData;
    public enum State 
    {
        Fleeing,
        SearchingFood,
        MovingIdle,
        lookingAround,
        Idle
    }
    public GameObject HuntedCorpse;
    public GameObject Corpse;
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
    public string Name;
    public string Parent;
    bool ShowData;
    public void OnMouseEnter()
    {
        ShowData = true;
    }
    public void OnMouseExit()
    {
        ShowData = false;
        AnimalData.text = "";
    }
    public void Start()
    {
        gameObject.name = Name;
        AnimalData = GameObject.Find("SelectedAnimalData").GetComponent<TMPro.TextMeshProUGUI>();
        Hunger = 0;
        age = 0;
        reprodLap = 0;
        reprodAge = Random.Range(25,80);
        body = gameObject.GetComponent<Rigidbody2D>();
        if (isProginator)
        {
            DNA = new Genes(20, 4, 2, 0, new Color(255,255,255));
            Name = AnimalNames.GenName();
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
        EnergyUsage = (DNA.Sight * StatModifiers.SightMod) + (DNA.Speed * StatModifiers.SpeedMod) + (DNA.Resilliance*StatModifiers.Resilliance) + (DNA.Damage * StatModifiers.Damage) +(diet.Count * 0.1f);
        int g=0;
        int r=0;
        int b=0;
        if (diet.Contains(FoodTypes.Plant))
        {
             g = 255;
        }
        if (diet.Contains(FoodTypes.Prey))
        {
             r = 255;
        }
        if (diet.Contains(FoodTypes.Carrion))
        {
             b = 255;
        }
        gameObject.GetComponent<SpriteRenderer>().color = new Color(r, g, b);
        transform.localScale = new Vector3(DNA.Resilliance/2, DNA.Damage/2 +DNA.Resilliance/2);
        gameObject.GetComponent<Food>().Nutrition = DNA.Resilliance * 50;
        if(diet.Contains(FoodTypes.Prey) && DNA.Damage == 0)
        {
            DNA.Damage = Random.Range(0, maxInclusive:1);
        }
        //target = transform.position = Random.insideUnitCircle * 3;
    }
    string GetDietAsString(List<FoodTypes> d)
    {
        string retStr = "";
        foreach(FoodTypes food in d)
        {
            
            retStr += food.ToString() + ", ";
        }
       
        return retStr;
    }
    
    public void Update()
    {
        
        if (ShowData)
        {
            
            
            AnimalData.text = $"{Name}\nAge: {Mathf.Round(age)}\nParent: {Parent}\nHunger:{Hunger}\nEnergy Usage:{EnergyUsage}\nGenes:\nSight:{DNA.Sight}\nSpeed:{DNA.Speed}\nDamage:{DNA.Damage}\nResilliance:{DNA.Resilliance}\nDiet:{GetDietAsString(diet)}";
        }
       
        reprodLap += Time.deltaTime;
        if(reprodLap > reprodAge && Hunger < 100)
        {
            Hunger += 45;
            reprodAge = Random.Range(25, 50);
            reprodLap = 0;
           var child = Instantiate(gameObject, transform.position + QuickMath.RandomVector(-5, 5), Quaternion.identity);
            var ChildGenes = child.GetComponent<Animal>();
            ChildGenes.isProginator = false;
            ChildGenes.DNA.Sight += Random.Range(-2, maxInclusive: 2);
            ChildGenes.DNA.Speed += Random.Range(-2, maxInclusive: 3);
            ChildGenes.DNA.Damage += Random.Range(-2, maxInclusive: 2);
            ChildGenes.DNA.Resilliance += Random.Range(-1, maxInclusive: 1);
            ChildGenes.DNA.color = DNA.color + new Color(Random.Range(-20, maxInclusive: 20), Random.Range(-20, maxInclusive: 20), Random.Range(-20, maxInclusive: 20));
            ChildGenes.Name = AnimalNames.GenName();
            ChildGenes.Parent = Name;
            if(Random.Range(0,101) > 65)
            {
                var AddOrRem = Mathf.Round(Random.Range(0, 2));
                if(AddOrRem == 0 && ChildGenes.diet.Count > 1)
                {
                    ChildGenes.diet.RemoveAt(0);
                }
                else if(AddOrRem == 1)
                {
                    var FoodType = Mathf.Round(Random.Range(0, 3));
                    if(FoodType == 0)
                    {
                        ChildGenes.diet.Add(FoodTypes.Plant);
                    }
                    if (FoodType == 1)
                    {
                        ChildGenes.diet.Add(FoodTypes.Prey);
                    }
                    if (FoodType == 2)
                    {
                        ChildGenes.diet.Add(FoodTypes.Carrion);
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

                if (i >= 359)
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
            if (seenEnems)
            {
                if (diet.Contains(FoodTypes.Prey) && seenEnems.collider.GetComponent<Food>().foodType == FoodTypes.Prey)
                {
                    body.velocity = transform.up * DNA.Speed*2;
                }
            }
            
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
        if (seenEnems)
        {
            if (seenEnems.collider.GetComponent<Animal>())
            {
                if (seenEnems.collider.GetComponent<Animal>().diet.Contains(GetComponent<Food>().foodType))
                {
                    target = transform.position - transform.up * DNA.Sight;
                    moveState = State.Fleeing;
                }
            }
        }
      
        if (Hunger > 150)
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
        var corpse = Instantiate(Corpse, transform.position, Quaternion.identity);
        corpse.transform.eulerAngles = transform.eulerAngles;
        corpse.transform.localScale = transform.localScale;
        Destroy(gameObject);
    }
    void DieHunted()
    {
        var corpse = Instantiate(HuntedCorpse, transform.position, Quaternion.identity);
        corpse.transform.eulerAngles = transform.eulerAngles;
        corpse.transform.localScale = transform.localScale;
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
                if(Random.Range(PreyGenes.DNA.Resilliance/2, PreyGenes.DNA.Resilliance) + DNA.Damage*2 > PreyGenes.DNA.Resilliance)
                {
                    col.GetComponent<Animal>().DieHunted();
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
    private void OnTriggerStay2D(Collider2D col)
    {
        if (diet.Contains(col.gameObject.GetComponent<Food>().foodType) && Hunger > 10 && moveState != State.Fleeing)
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

