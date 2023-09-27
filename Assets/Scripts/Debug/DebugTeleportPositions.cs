using HietakissaUtils.Commands;
using HietakissaUtils;
using UnityEngine;

public class DebugTeleportPositions : MonoBehaviour
{
    [SerializeField] Transform[] positions;
    [SerializeField] GameObject foodPrefab;

    void Awake()
    {
        CommandSystem.AddCommand(new DebugCommand<int>("tp", (int index) =>
        {
            if (!positions.IndexInBounds(index)) return;
            PlayerData.playerTransform.GetComponent<MovementController>().Teleport(positions[index].position);
        }));

        CommandSystem.AddCommand(new DebugCommand("spawnfood", () =>
        {
            Instantiate(foodPrefab, PlayerData.cameraTransform.position + PlayerData.cameraTransform.forward * 3f, Quaternion.identity);
        }));
    }
}
