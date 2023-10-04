using HietakissaUtils;
using UnityEngine;

public class InteractionController : MonoBehaviour
{
    [SerializeField] float interactionRange;
    [SerializeField] LayerMask interactionMask;
    [SerializeField] LayerMask worldspaceButtonMask;

    RaycastHit hit;

    void Awake()
    {
        PlayerData.interactionMask = interactionMask;
    }

    void Update()
    {
        if (PlayerData.usingBook)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Book.Instance.StopUsing();
                return;
            }

            Debug.DrawRay(PlayerData.cameraTransform.position, GetDirectionToMouse() * 5, Color.blue);

            if (Input.GetMouseButtonDown(0) && Physics.Raycast(PlayerData.cameraTransform.position, GetDirectionToMouse(), out hit, 5f, worldspaceButtonMask))
            {
                if (hit.collider.TryGetComponent(out WorldSpaceButton button)) button.Click();
            }
        }

        HandleUnGrabbingAndInteracting();
        HandleInteraction();
        

        void HandleUnGrabbingAndInteracting()
        {
            if (PlayerData.grabbingObject && Input.GetMouseButtonUp(0))
            {
                GrabbingController.Instance.UnGrabObject();
                //PlayerData.lastGrab.StopGrab();
                return;
            }
        }
        void HandleInteraction()
        {
            if (Physics.Raycast(PlayerData.cameraTransform.position, PlayerData.cameraTransform.forward, out hit, interactionRange, interactionMask))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (hit.collider.TryGetComponent(out IGrabbable grab))
                    {
                        PlayerData.lastGrab = grab;
                        PlayerData.lastGrabObject = hit.collider.gameObject;
                        PlayerData.lastGrabPoint = hit.point;

                        PlayerData.GrabIsDoor = hit.collider.TryGetComponent(out DynamicDoor door);

                        GrabbingController.Instance.GrabObject();
                        grab.StartGrab();
                    }
                }
                else if (Input.GetKeyDown(KeyCode.E) && hit.collider.TryGetComponent(out IInteractable interact)) interact.Interact();
            }
        }

        Vector3 GetDirectionToMouse()
        {
            return Maf.Direction(PlayerData.cameraTransform.position, PlayerData.playerCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1f)));
        }
    }

    void OnDrawGizmos()
    {
        if (!PlayerData.ValidatePlayerReferences()) return;

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
