using UnityEngine;

public class InteractionController : MonoBehaviour
{
    [SerializeField] float interactionRange;

    void Update()
    {
        if (PlayerData.grabbingObject && Input.GetMouseButtonUp(0))
        {
            PlayerData.lastGrab.StopGrab();
            return;
        }

        if (Physics.Raycast(PlayerData.cameraTransform.position, PlayerData.cameraTransform.forward, out RaycastHit hit, interactionRange))
        {
            DebugText.Instance.AddText("Can interact: True (not necessarily though, too lazy to fix)");

            if (Input.GetMouseButtonDown(0))
            {
                if (hit.collider.TryGetComponent(out IGrabbable grab))
                {
                    PlayerData.lastGrab = grab;
                    PlayerData.lastGrabObject = hit.collider.gameObject;

                    grab.StartGrab();
                }
            }
        }
        else DebugText.Instance.AddText("Can interact: False");
    }

    void OnDrawGizmos()
    {
        if (Physics.Raycast(PlayerData.cameraTransform.position, PlayerData.cameraTransform.forward, out RaycastHit hit, interactionRange) && hit.collider.TryGetComponent(out IGrabbable interactable))
        {
            Gizmos.color = Color.green;

            Gizmos.DrawLine(PlayerData.cameraTransform.position, PlayerData.cameraTransform.position + PlayerData.cameraTransform.forward * interactionRange);
        }
        else
        {
            Gizmos.color = Color.red;

            Gizmos.DrawLine(PlayerData.cameraTransform.position, PlayerData.cameraTransform.position + PlayerData.cameraTransform.forward * interactionRange);
        }
    }
}
