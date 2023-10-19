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

    /// <summary>
    /// tosi rumaa spagettia!!!
    /// toivottavasti ei tarvii ikin‰ en‰‰ koskee t‰h‰n koodiin (foreshadowing)
    /// </summary>

    void Update()
    {
        //if press esc > pause
        //if paused and press esc > unpause

        if (PlayerData.usingBook)
        {
            if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Escape))
            {
                Book.Instance.StopUsingBook();
                return;
            }

            Debug.DrawRay(PlayerData.cameraTransform.position, GetDirectionToMouse() * 5, Color.blue);

            if (Input.GetMouseButtonDown(0))
            {
                //if (hit.collider.TryGetComponent(out WorldSpaceButton button)) button.Click();
                if (Physics.Raycast(PlayerData.cameraTransform.position, GetDirectionToMouse(), out hit, 5f, worldspaceButtonMask))
                {
                    if (hit.collider.TryGetComponent(out WorldSpaceButton button)) button.Click();
                }
                /*else
                {
                    PointerEventData pointerData = new PointerEventData(EventSystem.current);
                    pointerData.position = Input.mousePosition;
                    List<RaycastResult> results = new List<RaycastResult>();
                    EventSystem.current.RaycastAll(pointerData, results);

                    foreach (RaycastResult result in results)
                    {
                        if (result.gameObject.TryGetComponent(out Button button)) button.OnPointerClick(pointerData);
                    }
                }*/
            }
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameManager.IsPaused && GameManager.GameRunning) GameManager.UnPause();
            else GameManager.Pause();
        }
        else
        {
            HandleUnGrabbingAndInteracting();
            HandleInteraction();
        }

        void HandleUnGrabbingAndInteracting()
        {
            if (PlayerData.grabbingObject && Input.GetMouseButtonUp(0))
            {
                Debug.Log("Ungrabbed object because mouse was released");
                GrabbingController.Instance.UnGrabObject();
                //PlayerData.lastGrab.StopGrab();
                return;
            }
        }
        void HandleInteraction()
        {
            //Physics.Raycast(PlayerData.cameraTransform.position, PlayerData.cameraTransform.forward, out hit, interactionRange, interactionMask)
            if (Physics.Raycast(PlayerData.cameraTransform.position, PlayerData.cameraTransform.forward, out hit, interactionRange, interactionMask))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    IGrabbable grab;
                    bool hasGrab = false;

                    if (hit.collider.TryGetComponent(out GrabBranch branch))
                    {
                        Debug.Log($"Grabbed branch of {branch.RootObject.name}");

                        IGrabbable root = branch.RootObject.GetComponent<IGrabbable>();

                        if (branch.RootObject.TryGetComponent(out Frog frog) && frog.StateIs(FrogState.Potty)) return;

                        PlayerData.lastGrab = root;
                        PlayerData.lastGrabObject = branch.RootObject;
                        grab = root;

                        PlayerData.GrabIsDoor = false;

                        hasGrab = true;
                    }
                    else if (hit.collider.TryGetComponent(out grab))
                    {
                        PlayerData.lastGrab = grab;
                        PlayerData.lastGrabObject = hit.collider.gameObject;

                        PlayerData.lastGrabPoint = hit.point;
                        PlayerData.GrabIsDoor = hit.collider.TryGetComponent(out DynamicDoor door);

                        hasGrab = true;
                    }

                    if (hasGrab)
                    {
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