using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform holder;

    [SerializeField] float sensitivity;

    [SerializeField] bool invertVertical;
    [SerializeField] bool invertHorizontal;

    [SerializeField] float maxLookAngle = 89f;

    float xRot, yRot;

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (PlayerData.usingBook) return;

        GetInput();
        Rotate();
    }

    void LateUpdate()
    {
        if (PlayerData.usingBook)
        {
            transform.position = PlayerData.bookLookTransform.position;
            transform.rotation = PlayerData.bookLookTransform.rotation;
        }
        else transform.position = holder.position;
    }

    void GetInput()
    {
        yRot += Input.GetAxisRaw("Mouse X") * (invertHorizontal ? -sensitivity : sensitivity);
        xRot += Input.GetAxisRaw("Mouse Y") * (invertVertical ? sensitivity : -sensitivity);

        xRot = Mathf.Clamp(xRot, -maxLookAngle, maxLookAngle);

        if (yRot > 360f) yRot -= 360f;
        else if (yRot < -360f) yRot += 360f;
    }

    void Rotate()
    {
        transform.rotation = Quaternion.Euler(xRot, yRot, 0f);
        PlayerData.playerTransform.rotation = Quaternion.Euler(0f, yRot, 0f);
    }
}
