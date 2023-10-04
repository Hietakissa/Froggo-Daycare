using UnityEngine;

public class FloaterController : MonoBehaviour
{
    Floater[] floaters;

    [HideInInspector] public LayerMask waterMask;
    [HideInInspector] public Rigidbody rb;

    void Awake()
    {
        waterMask = LayerMask.GetMask("Water");
        floaters = GetComponentsInChildren<Floater>();
        rb = GetComponent<Rigidbody>();

        foreach (Floater floater in floaters) floater.Init(this);
    }

    void FixedUpdate()
    {
        bool underwater = false;

        foreach (Floater floater in floaters)
        {
            if (floater.IsUnderwater())
            {
                underwater = true;
                floater.Process();
            }
        }

        if (underwater)
        {
            rb.drag = 2f;
            rb.angularDrag = 2f;
        }
        else
        {
            rb.drag = 0f;
            rb.angularDrag = 0.05f;
        }
        //rb.useGravity = !underwater;
    }
}
