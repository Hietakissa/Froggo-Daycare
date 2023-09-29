using UnityEngine;

public abstract class PlayerData
{
    public static IGrabbable lastGrab;
    public static GameObject lastGrabObject;
    public static bool grabbingObject;

    public static Camera playerCamera;

    public static Transform playerTransform;
    public static Transform cameraTransform;
    public static Transform cameraHolder;

    public static Transform bookLookTransform;

    public static bool usingBook;

    public static bool ValidatePlayerReferences()
    {
        return playerTransform && cameraTransform;
    }

    
}
