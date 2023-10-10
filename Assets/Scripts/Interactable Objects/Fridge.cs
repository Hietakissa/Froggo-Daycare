using HietakissaUtils;
using UnityEngine;

public class Fridge : Appliance
{
    [SerializeField] GameObject[] foodPrefabs;
    [SerializeField] Transform[] foodSpawnPositions;
    [SerializeField] int maxFoodAmount;
    int currentFoodAmount;

    void Awake()
    {
        CheckForMissingFood();
    }

    void DoorClosed()
    {
        //Debug.Log("Door closed");

        //if (!food) SpawnFood();
        /*if (!food)*/ CheckForMissingFood();
    }

    void AteFood()
    {
        currentFoodAmount--;

        //if (dynamicDoor.IsClosed) SpawnFood();
        if (dynamicDoor.IsClosed) CheckForMissingFood();
    }

    void CheckForMissingFood()
    {
        if (currentFoodAmount < 0) currentFoodAmount = 0;
        int missingFood = maxFoodAmount - currentFoodAmount;

        //Debug.Log($"{currentFoodAmount} food, {missingFood} missing");

        for (int i = 0; i < missingFood; i++)
        {
            SpawnFood();
        }
    }

    void SpawnFood()
    {
        currentFoodAmount++;

        Instantiate(foodPrefabs.RandomElement(), foodSpawnPositions.RandomElement().position, Quaternion.identity);
    }

    void OnEnable()
    {
        dynamicDoor.onDoorClosed += DoorClosed;
        GameManager.OnFoodEaten += AteFood;
    }

    void OnDisable()
    {
        dynamicDoor.onDoorClosed -= DoorClosed;
        GameManager.OnFoodEaten -= AteFood;
    }
}
