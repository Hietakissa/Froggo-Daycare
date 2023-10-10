using UnityEngine;

public class FrogFuriousState : FrogBaseState
{
    Vector3 velocity;

    float speed;

    public override void EnterState()
    {
        frog.DisableMovement = true;

        speed = frog.BounceSpeed;
        velocity = Random.insideUnitSphere * speed;

        //Debug.DrawRay(frog.rb.position, velocity, Color.black, 10f);

        frog.rb.interpolation = RigidbodyInterpolation.Interpolate;
        frog.rb.useGravity = false;
        GameManager.FrogEnterFurious();
    }

    public override void ExitState()
    {
        frog.DisableMovement = false;

        if (!frog.ShouldOverridePosition) frog.rb.useGravity = true;
        GameManager.FrogExitFurious();
    }

    public override void UpdateState()
    {
        if (frog.isGrabbed || frog.ShouldOverridePosition) return;

        if (Vector3.Distance(frog.rb.position, Vector3.up) >= 30f) frog.rb.position = Vector3.up;

        //do frog bouncing here

        //Vector3 randomPos = Random.insideUnitSphere;
        //randomPos.y = 0f;
        //frog.rb.position = randomPos;

        frog.rb.velocity = Vector3.zero;

        //Debug.DrawRay(frog.rb.position, velocity, Color.green);

        if (Physics.SphereCast(frog.rb.position, 0.3f, velocity, out RaycastHit hit, velocity.magnitude * Time.deltaTime + frog.BounceDistanceOffset, frog.FuriousBounceMask))
        {
            if (hit.collider.TryGetComponent(out Rigidbody otherRB)) otherRB.AddForce(velocity, ForceMode.Impulse);

            Vector3 randomOffset = Random.insideUnitSphere * 0.5f;
            randomOffset.y = 0f;

            velocity += frog.transform.InverseTransformDirection(randomOffset);
            velocity = Vector3.Reflect(velocity, hit.normal).normalized * speed;
            //Debug.DrawRay(hit.point, velocity, Color.red, 3f);
        }

        //frog.rb.position = frog.rb.position + velocity * Time.deltaTime;
        frog.transform.position = frog.transform.position + velocity * Time.deltaTime;
        frog.rb.rotation = Quaternion.Slerp(frog.rb.rotation, Quaternion.LookRotation(velocity), 10f * Time.deltaTime);
    }
}
