using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float sensitivity;

    [SerializeField] bool invertVertical;
    [SerializeField] bool invertHorizontal;

    [SerializeField] float maxLookAngle = 90f;

    Transform holder;

    float xRot, yRot;

    void Awake()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        Application.targetFrameRate = 144;

        PlayerData.cameraTransform = transform;
        PlayerData.playerCamera = GetComponent<Camera>();
    }

    void Start()
    {
        holder = PlayerData.cameraHolder;

        Book.Instance.StartUsing();
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
