using HietakissaUtils;
using UnityEngine;

public class Fridge : Appliance
{
    [SerializeField] GameObject[] foodPrefabs;
    [SerializeField] Transform[] foodSpawnPositions;

    GameObject food;

    void Awake()
    {
        SpawnFood();
    }

    void DoorClosed()
    {
        if (!food) SpawnFood();
    }

    void AteFood()
    {
        if (dynamicDoor.IsClosed) SpawnFood();
    }

    void SpawnFood()
    {
        food = Instantiate(foodPrefabs.RandomElement(), foodSpawnPositions.RandomElement().position, Quaternion.identity);
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
