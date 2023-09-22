using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float jumpForce;
    [SerializeField] float sphereCastRadius;

    [Header("Physics")]
    [SerializeField] float gravity;
    [SerializeField] float fallMultiplier;
    [SerializeField] float drag;

    CharacterController cc;

    float horizontal, vertical;

    Vector3 moveDir;
    Vector3 velocity;

    const float Gravity = 9.81f;

    [SerializeField] float sphereCastOffset = 0.05f;
    [SerializeField] float rayCastOffset = 0.05f;

    RaycastHit sphereCast;
    RaycastHit groundRay;

    bool isGrounded;

    void Awake()
    {
        cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) Time.timeScale = 0.1f;
        else if (Input.GetKeyDown(KeyCode.T)) Time.timeScale = 1f;

        /*if (Input.GetKeyDown(KeyCode.K))
        {
            //Knockback doesn't fully work, no drag so it keeps adding up



            float knockbackForce = 5f;

            Vector3 knockback = (Vector3.right + Vector3.up).normalized * knockbackForce;
            velocity += knockback;
        }*/


        //TODO Clean up code some day


        //Input
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");


        //Grounded check
        isGrounded = Physics.SphereCast(transform.position + Vector3.up * (sphereCastRadius + sphereCastOffset), sphereCastRadius, Vector3.down, out sphereCast, sphereCastOffset * 2);
        Physics.Raycast(transform.position + Vector3.up * rayCastOffset, Vector3.down, out groundRay, rayCastOffset * 2);


        //Project move dir, TODO somehow orient vector away from the slope
        //incorrect at certain angles in stairs due to the projection, should probably fix at some point
        moveDir = (PlayerData.playerTransform.forward * vertical + PlayerData.playerTransform.right * horizontal);
        moveDir.y = 0f;
        moveDir = Vector3.ProjectOnPlane(moveDir, groundRay.normal);
        moveDir = moveDir.normalized * speed;

        Debug.DrawLine(groundRay.point, groundRay.point + groundRay.normal, Color.red);

        //Gravity
        if (!isGrounded) velocity += Vector3.down * CalculateGravityMagnitude();
        else if (velocity.y <= 0)
        {
            velocity.y = 0;
        }


        //Drag, terrible implementation, technically works, will leave it at that for now
        Vector3 tempVelocity = velocity;
        tempVelocity.y = 0f;
        if (tempVelocity.magnitude < 0.1f && velocity.y == 0f) velocity = Vector3.zero;
        else velocity -= tempVelocity.normalized * drag * Time.deltaTime;
        //velocity -= Vector3.Cross(-velocity.normalized, Vector3.one * drag) * Time.deltaTime;


        //Jumping, TODO add coyote time, jump buffer, analog jump thing
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded) velocity.y = jumpForce;


        //Move
        cc.Move((moveDir + velocity) * Time.deltaTime);


        //Debug
        DebugText.Instance.AddText($"Velocity: {velocity}");
        DebugText.Instance.AddText($"Total move amount: {moveDir + velocity}");
        DebugText.Instance.AddText($"Grounded: {isGrounded}");
        DebugText.Instance.AddText($"Move force: {speed * Time.deltaTime}");
        DebugText.Instance.AddText($"Gravity force: {CalculateGravityMagnitude()}");
        DebugText.Instance.AddText($"Ground angle: {Vector3.Angle(groundRay.normal, Vector3.up)}");
    }

    float CalculateGravityMagnitude() => Gravity * gravity * (velocity.y < 0 ? fallMultiplier : 1) * Time.deltaTime;

    public void AddKnockback(Vector3 knockbackForce)
    {
        velocity += knockbackForce;
    }

    public void Teleport(Vector3 position)
    {
        cc.enabled = false;
        transform.position = position;
        cc.enabled = true;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawRay(transform.position + Vector3.up * 0, moveDir);

        if (Physics.SphereCast(transform.position + Vector3.up * (sphereCastRadius + sphereCastOffset), sphereCastRadius, Vector3.down, out sphereCast, sphereCastOffset * 2))
        {
            Gizmos.color = Color.green;

            Gizmos.DrawWireSphere(new Vector3(transform.position.x, sphereCast.point.y, transform.position.z) + Vector3.up * sphereCastRadius, sphereCastRadius);
        }
        else
        {
            Gizmos.color = Color.red;

            Gizmos.DrawWireSphere(transform.position + Vector3.down * (sphereCastOffset - sphereCastRadius), sphereCastRadius);
        }
    }
}
