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
    public static float NumPlants;
    // Start is called before the first frame update
    void Start()
    {
        Lifespan = 0;
        reProdSpan = Random.Range(30, 95);
        Deathspan = reProdSpan - 10 +Random.Range(25, 45);
        if(foodType == FoodTypes.Carrion)
        {
            Deathspan += 150;
            Nutrition += transform.localScale.x * 30;
        }
        if(foodType == FoodTypes.Plant)
        {
            Food.NumPlants += 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Lifespan += Time.deltaTime;
        if(foodType == FoodTypes.Plant && Lifespan > reProdSpan && !HasReproduced)
        {
            var NumPlantsMake = Random.Range(1, 3);
            if(NumPlantsMake + NumPlants < 650)//650 is plant cap;
            {
                HasReproduced = true;
                for (int i = 0; i < NumPlantsMake; i++)
                {
                    Instantiate(gameObject, transform.position + QuickMath.RandomVector(-30, 30), Quaternion.identity);
                }
            }
           
        }
        if((foodType == FoodTypes.Plant || foodType == FoodTypes.Carrion) && Lifespan > Deathspan)
        {
            Destroy(gameObject);
        }
    }
    private void OnDestroy()
    {
        if (foodType == FoodTypes.Plant)
        {
            Food.NumPlants -= 1;
        }
    }
}
