using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum FoodTypes
{
    Plant,
    Prey,
    Carrion
}
public class Food : MonoBehaviour
{
    public FoodTypes foodType;
    public float Nutrition;
    public float Lifespan;//Plants only
    float reProdSpan;//Plants Only
    float Deathspan;//Plants and Carrion only
    bool HasReproduced;//Plants
    // Start is called before the first frame update
    void Start()
    {
        Lifespan = 0;
        reProdSpan = Random.Range(20, 85);
        Deathspan = reProdSpan - 10 +Random.Range(5, 25);
        if(foodType == FoodTypes.Carrion)
        {
            Deathspan += 150;
            Nutrition += transform.localScale.x * 20;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Lifespan += Time.deltaTime;
        if(foodType == FoodTypes.Plant && Lifespan > reProdSpan && !HasReproduced)
        {
            HasReproduced = true;
            for(int i =0; i<Random.Range(1, 5); i++)
            {
                Instantiate(gameObject, transform.position + QuickMath.RandomVector(-10, 10), Quaternion.identity);
            }
        }
        if((foodType == FoodTypes.Plant || foodType == FoodTypes.Carrion) && Lifespan > Deathspan)
        {
            Destroy(gameObject);
        }
    }
}
