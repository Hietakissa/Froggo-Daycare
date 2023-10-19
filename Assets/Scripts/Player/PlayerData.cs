using UnityEngine;

public abstract class PlayerData
{
    //joo t‰‰ll‰ on asioita mit‰ ei tarvis mut menis liikaa aikaa teh‰ hyvin

    public static IGrabbable lastGrab;
    public static GameObject lastGrabObject;
    public static bool grabbingObject;
    public static Vector3 lastGrabPoint;

    public static bool GrabIsDoor;

    public static Camera playerCamera;

    public static Transform playerTransform;
    public static Transform cameraTransform;
    public static Transform cameraHolder;

    public static Transform bookLookTransform;

    public static bool usingBook;

    public static LayerMask interactionMask;

    public static float sensitivity;
    public static int masterVolume;
    public static int musicVolume;

    public static bool ValidatePlayerReferences()
    {
        return playerTransform && cameraTransform;
    }

    
}
