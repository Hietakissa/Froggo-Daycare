using UnityEngine;

public class FloaterController : MonoBehaviour
{
    Floater[] floaters;

    [SerializeField] float uprightTorque = 60;
    [SerializeField] float uprightTorqueDamping = 2;

    [HideInInspector] public LayerMask waterMask;
    [HideInInspector] public Rigidbody rb;

    [HideInInspector] public bool underwater;
    bool lastUnderwater;

    float lastSplashTime;

    void Awake()
    {
        waterMask = LayerMask.GetMask("Water");
        floaters = GetComponentsInChildren<Floater>();
        rb = GetComponent<Rigidbody>();

        foreach (Floater floater in floaters) floater.Init(this);
    }

    void FixedUpdate()
    {
        underwater = false;

        foreach (Floater floater in floaters)
        {
            if (floater.IsUnderwater())
            {
                underwater = true;
                floater.Process();
            }
        }

        if (underwater != lastUnderwater && Time.time - lastSplashTime > 0.5f)
        {
            SoundManager.Instance.PlaySplashSound(transform.position);
            lastSplashTime = Time.time;
        }

        if (underwater)
        {
            rb.drag = 2f;
            rb.angularDrag = 2f;

            Quaternion difference = Quaternion.FromToRotation(transform.up, Vector3.up);
            float angle;
            Vector3 axis;

            difference.ToAngleAxis(out angle, out axis);

            rb.AddTorque(-rb.angularVelocity * uprightTorqueDamping, ForceMode.Acceleration);
            rb.AddTorque(axis.normalized * angle * uprightTorque * Time.deltaTime, ForceMode.Acceleration);
        }
        else
        {
            rb.drag = 0f;
            rb.angularDrag = 0.05f;
        }

        lastUnderwater = underwater;
    }
}
