using System.Collections;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float runMultiplier;
    [SerializeField] float jumpForce;
    [SerializeField] float crouchHeight;
    [SerializeField] float crouchMultiplier;

    [Header("QOL / Input")]
    [SerializeField] float coyoteTime;
    [SerializeField] float jumpBufferTime;
    [SerializeField] bool allowHoldingJump;
    [SerializeField] bool keepCrouchPressedAirborne;

    [Header("Physics")]
    [SerializeField] float gravity;
    [SerializeField] float fallMultiplier;
    [SerializeField] float drag;

    CharacterController cc;

    float horizontal, vertical;

    Vector3 moveDir;
    Vector3 velocity;

    const float Gravity = 9.81f;

    [SerializeField] float sphereCastRadius;
    [SerializeField] float sphereCastOffset = 0.05f;
    [SerializeField] float rayCastOffset = 0.05f;

    RaycastHit sphereCast;
    RaycastHit groundRay;

    bool isGrounded;
    bool lastGrounded;
    bool isCrouching;
    bool crouchKeyHeld;

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
        //Grounded check
        isGrounded = Physics.SphereCast(transform.position + Vector3.up * (sphereCastRadius + sphereCastOffset), sphereCastRadius, Vector3.down, out sphereCast, sphereCastOffset * 2);
        Physics.Raycast(transform.position + Vector3.up * rayCastOffset, Vector3.down, out groundRay, rayCastOffset * 2);

        if (isGrounded && !lastGrounded) StartGround();
        else if (!isGrounded && lastGrounded) StartAir();

        HandlePhysics();
        GetInput();
        HandleMovement();

        if (PlayerData.usingBook) return;

        HandleCrouching();
        HandleJumping();

        HandleDebug();

        lastGrounded = isGrounded;

        void HandlePhysics()
        {
            HandleDrag();
            HandleGravity();

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
                velocity += Vector3.down * CalculateGravityMagnitude();
                if (isGrounded) velocity.y = Mathf.Max(-5f, velocity.y);
            }
        }

        void GetInput()
        {
            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");
        }

        void HandleMovement()
        {
            if (PlayerData.usingBook) moveDir = Vector3.zero;
            else MoveDir();
            cc.Move((moveDir + velocity) * Time.deltaTime);

            void MoveDir()
            {
                //TODO somehow orient vector away from the slope
                //joo ruma ja pitkä rivi koodia, en jaksa tehä hienompaa rn
                if (OnSteepGround()) moveDir = Vector3.ProjectOnPlane(Vector3.down, groundRay.normal);
                else moveDir = (PlayerData.playerTransform.forward * vertical + PlayerData.playerTransform.right * horizontal).normalized; 
                moveDir *= speed * GetSpeedMultiplier();



                float GetSpeedMultiplier()
                {
                    if (isCrouching) return crouchMultiplier;
                    else if (Input.GetKey(KeyCode.LeftShift)) return runMultiplier;
                    else return 1;
                }
            }
        }

        void HandleCrouching()
        {
            if (Input.GetKeyDown(KeyCode.LeftControl)) crouchKeyHeld = true;
            else if (Input.GetKeyUp(KeyCode.LeftControl)) crouchKeyHeld = false;

            if (!isGrounded)
            {
                if (isCrouching) EndCrouch();
                return;
            }

            //crouchKeyHeld = Input.GetKey(KeyCode.LeftControl);

            if (crouchKeyHeld && isCrouching) return;

            if (crouchKeyHeld && !isCrouching)
            {
                StartCrouch();
            }
            else if (isCrouching)
            {
                if (!Physics.SphereCast(transform.position + Vector3.up * (sphereCastRadius + sphereCastOffset), sphereCastRadius, Vector3.up, out RaycastHit hit, 2f - sphereCastRadius * 2 - sphereCastOffset))
                {
                    EndCrouch();
                }
            }

            //if (Input.GetKeyDown(KeyCode.LeftControl)) StartCrouch();
            //else if (Input.GetKeyUp(KeyCode.LeftControl)) EndCrouch();

            void StartCrouch()
            {
                isCrouching = true;

                //cc.height = crouchHeight;
                //cc.center = new Vector3(0f, 1f - (2f - crouchHeight) / 2f, 0f);

                //transform.localScale = new Vector3(1f, crouchHeight / 2f, 1f);
                cc.enabled = false;
                transform.localScale = new Vector3(1f, crouchHeight, 1f);
                cc.enabled = true;
                //Teleport(transform.position - new Vector3(0f, (2f - crouchHeight) / 2f, 0f));
            }

            void EndCrouch()
            {
                isCrouching = false;

                //cc.height = 2f;
                //cc.center = new Vector3(0f, 1f, 0f);
                cc.enabled = false;
                transform.localScale = new Vector3(1f, 1f, 1f);
                cc.enabled = true;
                //Teleport(transform.position + new Vector3(0f, (2f - crouchHeight) / 2f, 0f));
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
                    StopJumpBuffer();
                    jumpBufferCoroutine = StartCoroutine(SetJumpBufferCoroutine());
                }
            }
            else if (jumpBuffer)
            {
                if (CanJump()) Jump();
            }

            //bool CanJump() => coyote || (isGrounded && Vector3.Angle(groundRay.normal, Vector3.up) <= cc.slopeLimit);
            bool CanJump() => (coyote || (isGrounded && !OnSteepGround()) && !isCrouching);

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
                jumpBuffer = true;
                yield return new WaitForSeconds(jumpBufferTime);
                jumpBuffer = false;
            }
        }



        void HandleDebug()
        {
            Debug.DrawRay(transform.position, Vector3.down * rayCastOffset);

            //DebugText.Instance.AddText($"Velocity: {velocity}");
            //DebugText.Instance.AddText($"Total move amount: {moveDir + velocity}");
            DebugText.Instance.AddText($"Grounded: {isGrounded}");
            //DebugText.Instance.AddText($"Move force: {speed * Time.deltaTime}");
            //DebugText.Instance.AddText($"Gravity force: {CalculateGravityMagnitude()}");
            DebugText.Instance.AddText($"Ground angle: {GetGroundAngle()}");
            DebugText.Instance.AddText($"Coyote: {coyote}, Jump Buffer: {jumpBuffer}");
            DebugText.Instance.AddText($"Steep ground: {OnSteepGround()}");
        }



        void StartAir()
        {
            if (lastGrounded && !isGrounded && velocity.y < 0) velocity.y = 0f;

            if (!keepCrouchPressedAirborne) crouchKeyHeld = false;
        }

        void StartGround()
        {

        }
    }

    bool OnSteepGround()
    {
        return GetGroundAngle() > cc.slopeLimit;
    }

    float GetGroundAngle()
    {
        float raycastAngle = Vector3.Angle(groundRay.normal, Vector3.up);
        float spherecastAngle = Vector3.Angle(sphereCast.normal, Vector3.up);
        if (raycastAngle < spherecastAngle) return raycastAngle;
        else return spherecastAngle;
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
