using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance;

    Dictionary<Rigidbody, VelocityPair> rigidbodies = new Dictionary<Rigidbody, VelocityPair>();
    List<PauseRB> pauseRBs = new List<PauseRB>();

    void Awake()
    {
        Instance = this;
    }

    public void RegisterRigidbody(Rigidbody rb)
    {
        //rigidbodies.Add(rb, new VelocityPair(rb.velocity, rb.angularVelocity));
        pauseRBs.Add(new PauseRB(rb));
    }

    public void UnregisterRigidbody(Rigidbody rb)
    {
        //rigidbodies.Remove(rb);
        for (int i = pauseRBs.Count - 1; i >= 0; i--)
        {
            if (pauseRBs[i].rb == rb)
            {
                pauseRBs.RemoveAt(i);
                return;
            }
        }

        Debug.Log($"Tried to remove rigidbody '{rb.gameObject.name}' from the pause list, but it wasn't registered!");
    }

    void Pause()
    {
        //foreach (KeyValuePair<Rigidbody, VelocityPair> pair in rigidbodies)
        //{
        //    Rigidbody rb = pair.Key;

        //    Debug.Log($"pausing rigidbody with velocities: {rb.velocity}, ang: {rb.angularVelocity}");
        //    rb.constraints = RigidbodyConstraints.FreezeAll;
        //    pair.Value.SetVelocities(rb.velocity, rb.angularVelocity);
        //}

        foreach (PauseRB pauseRB in pauseRBs)
        {
            pauseRB.velocity = pauseRB.rb.velocity;
            pauseRB.angularVelocity = pauseRB.rb.angularVelocity;
            pauseRB.rb.constraints = RigidbodyConstraints.FreezeAll;

            Debug.Log($"pausing rigidbody with velocities: {pauseRB.velocity}, ang: {pauseRB.angularVelocity}");
        }
    }

    void UnPause()
    {
        //foreach (KeyValuePair<Rigidbody, VelocityPair> pair in rigidbodies)
        //{
        //    Rigidbody rb = pair.Key;
        //    VelocityPair velPair = rigidbodies[rb];

        //    Debug.Log($"unpausing rigidbody with velocities: {pair.Value.velocity}, ang: {pair.Value.angularVelocity}");

        //    rb.constraints = RigidbodyConstraints.None;
        //    rb.AddForce(velPair.velocity, ForceMode.VelocityChange);
        //    rb.angularVelocity = velPair.angularVelocity;
        //}

        foreach (PauseRB pauseRB in pauseRBs)
        {
            pauseRB.rb.constraints = RigidbodyConstraints.None;
            pauseRB.rb.velocity = pauseRB.velocity;
            pauseRB.rb.angularVelocity = pauseRB.angularVelocity;

            Debug.Log($"unpausing rigidbody with velocities: {pauseRB.velocity}, ang: {pauseRB.angularVelocity}");
        }
    }

    void OnEnable()
    {
        GameManager.OnPause += Pause;
        GameManager.OnUnPause += UnPause;
    }

    void OnDisable()
    {
        GameManager.OnPause -= Pause;
        GameManager.OnUnPause -= UnPause;
    }
}

class PauseRB
{
    public Rigidbody rb;
    public Vector3 velocity;
    public Vector3 angularVelocity;

    public PauseRB(Rigidbody rb)
    {
        this.rb = rb;
    }

    public void SetVelocities(Vector3 velocity, Vector3 angularVelocity)
    {
        this.velocity = velocity;
        this.angularVelocity = angularVelocity;
    }
}

class VelocityPair
{
    public Vector3 velocity;
    public Vector3 angularVelocity;

    public VelocityPair(Vector3 velocity, Vector3 angularVelocity)
    {
        this.velocity = velocity;
        this.angularVelocity = angularVelocity;
    }

    public void SetVelocities(Vector3 velocity, Vector3 angularVelocity)
    {
        this.velocity = velocity;
        this.angularVelocity = angularVelocity;
    }
}