using Unity.Mathematics;
using UnityEngine;

public class FloaterController : MonoBehaviour
{
    Floater[] floaters;

    [SerializeField] float uprightTorque = 60;
    [SerializeField] float uprightTorqueDamping = 2;
    [SerializeField] Transform rippleEffect;
    [Tooltip("Ripple Effect Size is about 2 from the small rubber duck, for example. 3.5 for a frog.")]
    [SerializeField] float rippleEffectSize = 2f;

    [HideInInspector] public LayerMask waterMask;
    [HideInInspector] public Rigidbody rb;

    [HideInInspector] public bool underwater;
    bool lastUnderwater;

    float lastSplashTime;
    bool ripplePrefabMissing; //ripple could be null so but we don't need the red error
    bool gotRipple; //Water effects instantiated once already
    bool skimming; //Making costant contact with the BathwaterSurface
    bool inTriggerSurface; //Used to get the inverse of OnTriggerStay
    Transform currentRippleTransform; //Instantiated water effect
    ParticleSystem currentRippleParticleSystem; //its PS

    void Awake()
    {
        waterMask = LayerMask.GetMask("Water");
        floaters = GetComponentsInChildren<Floater>();
        rb = GetComponent<Rigidbody>();

        foreach (Floater floater in floaters) floater.Init(this);

        if (rippleEffect == null)
        {
            Debug.LogWarning($"{gameObject} has unassigned Ripple effect prefab, will not get ripples when in water");
            ripplePrefabMissing = true;
        }
    }

    void FixedUpdate()
    {
        skimming = inTriggerSurface;

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
            //Debug.Log($"<color=2BC3FF> {gameObject} floating! Water height: {BathTubWater.WaterHeight}</color>");


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


        inTriggerSurface = false;
        lastUnderwater = underwater;
    }

    void Update()
    {
        if (!ripplePrefabMissing)
        {
            if (!gotRipple)
            {
                //if (rippleEffect != null)
                currentRippleTransform = Instantiate(rippleEffect, transform.position, quaternion.identity);
                currentRippleParticleSystem = currentRippleTransform.GetComponent<ParticleSystem>();
                var main = currentRippleParticleSystem.main;
                main.startSize = rippleEffectSize;
                gotRipple = true;
            }
            else
            {
                if (skimming)
                {
                    if (!currentRippleParticleSystem.isEmitting)
                        currentRippleParticleSystem.Play();


                    currentRippleTransform.position = new Vector3(
                        transform.position.x,
                        BathTubWater.WaterHeight,
                        transform.position.z);
                }
                else
                {
                    if (currentRippleParticleSystem.isEmitting)
                    {
                        currentRippleParticleSystem.Stop();
                    }
                }

            }
        }
    }

    /* void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WaterSurface"))
        {
            skimming = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("WaterSurface"))
        {
            skimming = false;
        }
    } */
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("WaterSurface"))
        {
            inTriggerSurface = true;
        }
    }
}
