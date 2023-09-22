using HietakissaUtils.Commands;
using HietakissaUtils;
using UnityEngine;

public class DebugTeleportPositions : MonoBehaviour
{
    [SerializeField] Transform[] positions;

    void Awake()
    {
        CommandSystem.AddCommand(new DebugCommand<int>("tp", (int index) =>
        {
            if (!positions.IndexInBounds(index)) return;
            PlayerData.playerTransform.GetComponent<MovementController>().Teleport(positions[index].position);
        }));
    }
}
