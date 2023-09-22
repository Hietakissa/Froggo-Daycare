using UnityEngine;

public abstract class PlayerData
{
    public static IGrabbable lastGrab;
    public static GameObject lastGrabObject;
    public static bool grabbingObject;

    public static Transform playerTransform;
    public static Transform cameraTransform;
}
