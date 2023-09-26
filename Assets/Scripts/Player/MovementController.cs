using System.Collections;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float runMultiplier;
    [SerializeField] float jumpForce;
    [SerializeField] float sphereCastRadius;

    [Header("QOL / Input")]
    [SerializeField] float coyoteTime;
    [SerializeField] float jumpBufferTime;
    [SerializeField] bool allowHoldingJump;

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
    bool lastGrounded;

    bool coyote;
    bool jumpBuffer;
    float timeInAir;
    Coroutine jumpBufferCoroutine;

    void Awake()
    {
        cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) Time.timeScale = 0.1f;
        else if (Input.GetKeyDown(KeyCode.T)) Time.timeScale = 1f;

        //Grounded check
        isGrounded = Physics.SphereCast(transform.position + Vector3.up * (sphereCastRadius + sphereCastOffset), sphereCastRadius, Vector3.down, out sphereCast, sphereCastOffset * 2);
        Physics.Raycast(transform.position + Vector3.up * rayCastOffset, Vector3.down, out groundRay, rayCastOffset * 2);


        HandlePhysics();
        GetInput();
        HandleMovement();
        HandleJumping();

        HandleDebug();

        lastGrounded = isGrounded;

        void HandlePhysics()
        {
            HandleDrag();
            HandleGravity();
        }

        void HandleDrag()
        {
            //terrible implementation, technically works, will leave it at that for now
            //the implementation isn't the greatest, so it slows down at an odd rate, should probably multiply it with 
            //Mathf.Max(1f, velocity.Magnitude()) or something similar
            Vector3 tempVelocity = velocity;
            tempVelocity.y = 0f;
            if (tempVelocity.magnitude < 0.1f && velocity.y == 0f) velocity = Vector3.zero;
            else velocity -= tempVelocity.normalized * drag * Time.deltaTime;
        }

        void HandleGravity()
        {
            if (lastGrounded && !isGrounded && velocity.y < 0) velocity.y = 0f;

            velocity += Vector3.down * CalculateGravityMagnitude();
            if (isGrounded) velocity.y = Mathf.Max(-5f, velocity.y);
        }

        void GetInput()
        {
            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");
        }

        void HandleMovement()
        {
            MoveDir();
            cc.Move((moveDir + velocity) * Time.deltaTime);

            void MoveDir()
            {
                //TODO somehow orient vector away from the slope
                //joo ruma ja pitkä rivi koodia, en jaksa tehä hienompaa rn
                moveDir = (PlayerData.playerTransform.forward * vertical + PlayerData.playerTransform.right * horizontal).normalized * (Input.GetKey(KeyCode.LeftShift) ? speed * runMultiplier: speed);
            }
        }

        void HandleJumping()
        {
            HandleCoyote();

            if (Input.GetKeyDown(KeyCode.Space) || (allowHoldingJump && Input.GetKey(KeyCode.Space)))
            {
                if (CanJump()) Jump();
                else if (!allowHoldingJump)
                {
                    Debug.Log("In the air, tried to jump but cant, resetting jump buffer");
                    StopJumpBuffer();
                    jumpBufferCoroutine = StartCoroutine(SetJumpBufferCoroutine());
                }
            }
            else if (jumpBuffer)
            {
                if (CanJump()) Jump();
            }

            bool CanJump() => coyote || (isGrounded && Vector3.Angle(groundRay.normal, Vector3.up) <= cc.slopeLimit);

            void Jump()
            {
                velocity.y = jumpForce;

                coyote = false;

                StopJumpBuffer();
            }

            void HandleCoyote()
            {
                if (lastGrounded && !isGrounded && velocity.y <= 0f) coyote = true;
                else if (!isGrounded)
                {
                    timeInAir += Time.deltaTime;

                    if (timeInAir > coyoteTime) coyote = false;
                }
                else
                {
                    timeInAir = 0f;
                    coyote = false;
                }
            }

            void StopJumpBuffer()
            {
                if (jumpBufferCoroutine != null) StopCoroutine(jumpBufferCoroutine);
                jumpBuffer = false;
            }

            IEnumerator SetJumpBufferCoroutine()
            {
                Debug.Log("Started jump buffer");
                jumpBuffer = true;
                Debug.Log($"Waiting for {jumpBufferTime} seconds");
                yield return new WaitForSeconds(jumpBufferTime);
                jumpBuffer = false;
                Debug.Log("Ended jump buffer");
            }
        }

        void HandleDebug()
        {
            DebugText.Instance.AddText($"Velocity: {velocity}");
            DebugText.Instance.AddText($"Total move amount: {moveDir + velocity}");
            DebugText.Instance.AddText($"Grounded: {isGrounded}");
            DebugText.Instance.AddText($"Move force: {speed * Time.deltaTime}");
            DebugText.Instance.AddText($"Gravity force: {CalculateGravityMagnitude()}");
            DebugText.Instance.AddText($"Ground angle: {Vector3.Angle(groundRay.normal, Vector3.up)}");
            DebugText.Instance.AddText($"Coyote: {coyote}, Jump Buffer: {jumpBuffer}");
        }
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
