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
      
        MovingIdle,
        lookingAround,
        Sleeping,
        Hunting
        
    }
    public GameObject Orient;
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
    bool IsSleeping;
    public GameObject Claws;
    public GameObject Legs;
    public GameObject Eyes;
    public List<Genes.Optimizers> optimizers;
    public Vector3 Basescale;//Used for flipping
  
    public delegate float optiDelegate(List<Genes.Optimizers> optimizers, Genes.Optimizers targetOpti);
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
        Basescale = transform.localScale;
        gameObject.name = Name;
        AnimalData = GameObject.Find("SelectedAnimalData").GetComponent<TMPro.TextMeshProUGUI>();
        Hunger = 0;
        age = 0;
        reprodLap = 0;
        reprodAge = Random.Range(25,80);
        body = gameObject.GetComponent<Rigidbody2D>();
        if (isProginator)
        {
            DNA = new Genes(20, 4, 2, 0, new Color(255,255,255),0,65);
            Name = AnimalNames.GenName();
        }
        if (isPlant)
        {
            DNA.Speed = 0;
        }
        DNA.Speed -= DNA.Resilliance / 2;
        if(DNA.Speed <= 0)
        {
            DNA.Speed = Random.Range(0, maxInclusive:1) ;
        }
        EnergyUsage = (DNA.Sight * StatModifiers.SightMod) + (DNA.Speed * StatModifiers.SpeedMod) + (DNA.Resilliance*StatModifiers.Resilliance) + (DNA.Damage * StatModifiers.Damage) +(diet.Count * 0.4f);
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
        transform.localScale = new Vector3(DNA.Resilliance*1.5f, DNA.Resilliance);
        gameObject.GetComponent<Food>().Nutrition = DNA.Resilliance * 50;
        Eyes.transform.localScale = new Vector3(DNA.Sight / 30, DNA.Sight / 30);
        Legs.transform.localScale = new Vector3(DNA.Speed*0.1f, DNA.Speed*0.2f);
        Claws.transform.localScale = new Vector3(DNA.Damage*0.5f, DNA.Damage);
        if (diet.Contains(FoodTypes.Prey) && DNA.Damage == 0)
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
    string GetOptimizersAsString(List<Genes.Optimizers> o)
    {
        string retStr = "";
        foreach (Genes.Optimizers opt in o)
        {

            retStr += opt.ToString() + ", ";
        }

        return retStr;
    }

    private void ResetUsage()
    {
        EnergyUsage = (DNA.Sight * StatModifiers.SightMod) + (DNA.Speed * StatModifiers.SpeedMod) + (DNA.Resilliance * StatModifiers.Resilliance) + (DNA.Damage * StatModifiers.Damage) + (diet.Count * 0.4f);

    }
    public void Update()
    {
       
        if (target.x - transform.position.x > 0)
        {
            
            transform.localScale = new Vector3(-Basescale.x, Basescale.y);
        }
        else
        {
            
            transform.localScale = Basescale;
        }
        optiDelegate optis = (x, y) => {
            foreach (Genes.Optimizers opt in x)
            {
                if (opt == y)
                {
                    return GeneOptimizaiton(y);
                }
            }
            return 0;
        };

        if (World.world.elap < DNA.WakeTime || World.world.elap > DNA.SleepTime)
        {
            IsSleeping = true;
            moveState = State.Sleeping;
            body.velocity = new Vector2(0, 0);
            
        }
        else if (IsSleeping)
        {
            ResetUsage();
            moveState = State.lookingAround;
            IsSleeping = false;
        }

        if (moveState == State.Sleeping)
        {
            EnergyUsage = 0;
        }
        
        if (ShowData)
        {
            
            
            AnimalData.text = $"{Name}\nAge: {Mathf.Round(age)}\nParent: {Parent}\nHunger: {Hunger.ToString("F2")}\nEnergy Usage: {EnergyUsage.ToString("F2")}\nGenes:\nSight: {DNA.Sight.ToString("F2")}\nSpeed: {(DNA.Speed  +optis(optimizers,Genes.Optimizers.Ergonomics)).ToString("F2")}\nDamage: {(DNA.Damage + optis(optimizers, Genes.Optimizers.Claws)).ToString("F2")}\nResilliance: {(DNA.Resilliance + optis(optimizers, Genes.Optimizers.Shell)).ToString("F2")}\nDiet: {GetDietAsString(diet)}\nBonuses: {GetOptimizersAsString(optimizers)}";
        }
       
        if(moveState != State.Sleeping)
        {
            reprodLap += Time.deltaTime;
        }
       
        if(reprodLap > reprodAge && Hunger < 100 && moveState != State.Sleeping)
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
            ChildGenes.DNA.WakeTime += Random.Range(-3, maxInclusive: 3);
            ChildGenes.DNA.SleepTime += Random.Range(-3, maxInclusive: 3);
            //ChildGenes.DNA.color = DNA.color + new Color(Random.Range(-20, maxInclusive: 20), Random.Range(-20, maxInclusive: 20), Random.Range(-20, maxInclusive: 20));
            ChildGenes.Name = AnimalNames.GenName();
            ChildGenes.Parent = Name;
            if(Random.Range(0,101) > 80)
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
        if(moveState == State.Hunting)
        {
            Orient.transform.up = target - transform.position;
            body.velocity = Orient.transform.up * (DNA.Speed * 2 + optis(optimizers, Genes.Optimizers.Ergonomics));
            if (seenEnems)
            {
                if (diet.Contains(FoodTypes.Prey) && seenEnems.collider.GetComponent<Food>().foodType == FoodTypes.Prey)
                {
                    body.velocity = Orient.transform.up * (DNA.Speed * 2 + optis(optimizers, Genes.Optimizers.Ergonomics));
                }
            }

            if (Vector2.Distance(transform.position, target) < 0.1f)
            {
                i = 0;
                body.velocity = new Vector3(0, 0);
                if (diet.Contains(FoodTypes.Prey))
                {
                    moveState = State.lookingAround;
                }
                else
                {
                    moveState = State.MovingIdle;
                }
              

            }
        }
        if (moveState == State.lookingAround)
        {

                i += 1*DNA.Speed;
                Orient.transform.Rotate(new Vector3(0, 0, 1*DNA.Speed));
                if (seenEnems)
                {
                   
                    if (diet.Contains(seenEnems.collider.gameObject.GetComponent<Food>().foodType))
                    {
                       
                            target = seenEnems.transform.position;
                            moveState = State.Hunting;
                        
                      
                       
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
            Orient.transform.up = target - transform.position;
            body.velocity = Orient.transform.up * (DNA.Speed * 2 + optis(optimizers, Genes.Optimizers.Ergonomics));
            if (seenEnems)
            {
                if (diet.Contains(FoodTypes.Prey) && seenEnems.collider.GetComponent<Food>().foodType == FoodTypes.Prey)
                {
                    body.velocity = Orient.transform.up * (DNA.Speed + optis(optimizers, Genes.Optimizers.Ergonomics));
                }
            }
            
            if (Vector2.Distance(transform.position, target) < 0.1f)
            {
                i = 0;
                body.velocity = new Vector3(0, 0);
                if(Random.Range(Hunger, 150*DNA.Resilliance) > 150*DNA.Resilliance / 1.5f)
                {
                    moveState = State.lookingAround;
                }
                else
                {
                    target = transform.position + QuickMath.RandomVector(-10, 10);
                }
               
                
            }
            
            
        }
        if (seenEnems)
        {
            if (seenEnems.collider.GetComponent<Animal>())
            {
                if (seenEnems.collider.GetComponent<Animal>().diet.Contains(GetComponent<Food>().foodType))
                {
                    target = transform.position - Orient.transform.up * DNA.Speed;
                    moveState = State.Fleeing;
                }
            }
        }
       
        if (moveState == State.Fleeing)
        {
            Orient.transform.up = target - transform.position;
            body.velocity = Orient.transform.up * (DNA.Speed * 2 + optis(optimizers, Genes.Optimizers.Ergonomics));
            if (Vector2.Distance(transform.position, target) < 0.1f)
            {
                i = 0;
                body.velocity = new Vector3(0, 0);
                moveState = State.MovingIdle;

            }
            
            

        }
        age += Time.deltaTime;
        Hunger += EnergyUsage * Time.deltaTime;
        //transform.Rotate(new Vector3(0, 0, 1f));
        if(moveState != State.Fleeing && moveState != State.Sleeping)
        {
            seenEnems = Physics2D.Raycast(transform.position + Orient.transform.up * transform.localScale.y, Orient.transform.up, DNA.Sight - World.world.NightMod);
        }
        
        
      
        if (Hunger > 150 * DNA.Resilliance)
        {
            Die();
        }
        if(age > 600)
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
        Gizmos.DrawRay(transform.position, Orient.transform.up*(DNA.Sight-World.world.NightMod));
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
        //Very Efficient Yup Yup
        optiDelegate optis = (x, y) => {
            foreach (Genes.Optimizers opt in x)
            {
                if (opt == y)
                {
                    return GeneOptimizaiton(y);
                }
            }
            return 0;
        };
        if (diet.Contains(col.gameObject.GetComponent<Food>().foodType) && moveState == State.Hunting)
         {
            if (col.GetComponent<Animal>())
            {
                var PreyGenes = col.GetComponent<Animal>();
                if(PreyGenes.Parent != Name)
                {
                    if (Random.Range(PreyGenes.DNA.Resilliance + optis(PreyGenes.optimizers, Genes.Optimizers.Shell) / 2, PreyGenes.DNA.Resilliance + optis(PreyGenes.optimizers, Genes.Optimizers.Shell)) + DNA.Damage * 2 + optis(optimizers, Genes.Optimizers.Claws) > PreyGenes.DNA.Resilliance + optis(PreyGenes.optimizers, Genes.Optimizers.Shell))
                    {
                        col.GetComponent<Animal>().DieHunted();
                        Hunger -= col.gameObject.GetComponent<Food>().Nutrition;
                        if (Hunger < 0)
                        {
                            Hunger = 0;
                        }
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
    public float GeneOptimizaiton(Genes.Optimizers trait)
    {
        if (trait == Genes.Optimizers.Shell)
        {
            return DNA.Resilliance * 2;
        }
        if (trait == Genes.Optimizers.Claws)
        {
            return DNA.Damage * 2;
        }
        if (trait == Genes.Optimizers.Ergonomics)
        {
            return DNA.Speed * 2;
        }

        return 0;

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
    public float WakeTime;
    public float SleepTime;
    public enum Optimizers
    {
        Shell,
        Claws,
        Ergonomics,
        None
    }
   
    public Genes(float s, float sp, float r, float d, Color c, float wt, float st)
    {
        this.Sight = s;
        this.Speed = sp;
        this.color = c;
        this.Damage = d;
        this.Resilliance = r;
        this.WakeTime = wt;
        this.SleepTime = st;
        
    }

}
public static class StatModifiers
{
    public static readonly float SightMod = 0.15f/2;
    public static readonly float SpeedMod = 1f/2;
    public static readonly float Resilliance = 0.6f / 2;
    public static readonly float Damage = 1f / 2;
    
}
public static class QuickMath
{
    public static Vector3 RandomVector(float min, float max)
    {
        return new Vector3(Random.Range(min, max), Random.Range(min, max));
    }
}

