using UnityEngine;

public class Fridge : Appliance
{
    [SerializeField] GameObject foodPrefab;
    [SerializeField] Transform foodSpawnPos;

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
        food = Instantiate(foodPrefab, foodSpawnPos.position, Quaternion.identity);
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
