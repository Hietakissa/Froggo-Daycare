using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance;

    Dictionary<Rigidbody, VelocityPair> rigidbodies = new Dictionary<Rigidbody, VelocityPair>();
    List<PauseRB> pauseRBs = new List<PauseRB>();

    List<string> debugNames = new List<string>();

    void Awake()
    {
        Instance = this;
    }

    public void RegisterRigidbody(Rigidbody rb)
    {
        //rigidbodies.Add(rb, new VelocityPair(rb.velocity, rb.angularVelocity));
        pauseRBs.Add(new PauseRB(rb));
        debugNames.Add(rb.name);
    }

    public void UnregisterRigidbody(Rigidbody rb)
    {
        //rigidbodies.Remove(rb);
        for (int i = pauseRBs.Count - 1; i >= 0; i--)
        {
            if (pauseRBs[i].rb == rb)
            {
                pauseRBs.RemoveAt(i);
                debugNames.RemoveAt(i);
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

        for (int i = 0; i < pauseRBs.Count; i++)
        {
            PauseRB prb = pauseRBs[i];

            if (prb.rb == null)
            {
                Debug.Log($"Tried to pause a rigidbody that's been destroyed! {debugNames[i]}, no longer raises errors, but should be fixed (edit: cannob be fixed, won't be fixed, not a problem)");
                continue;
            }

            prb.velocity = prb.rb.velocity;
            prb.angularVelocity = prb.rb.angularVelocity;
            prb.rb.constraints = RigidbodyConstraints.FreezeAll;
        }

        /*foreach (PauseRB pauseRB in pauseRBs)
        {
            //if (pauseRB.rb == null)
            //{
            //    Debug.Log($"Tried to pause a rigidbody that's been destroyed! {pauseRB.owner.name}, {pauseRB.owner.transform.position}, no longer raises errors, but should be fixed");
            //    continue;
            //}

            pauseRB.velocity = pauseRB.rb.velocity;
            pauseRB.angularVelocity = pauseRB.rb.angularVelocity;
            pauseRB.rb.constraints = RigidbodyConstraints.FreezeAll;

            Debug.Log($"pausing rigidbody with velocities: {pauseRB.velocity}, ang: {pauseRB.angularVelocity}");
        }*/
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

        for (int i = 0; i < pauseRBs.Count; i++)
        {
            PauseRB prb = pauseRBs[i];

            if (prb.rb == null)
            {
                Debug.Log($"Tried to unpause a rigidbody that's been destroyed! {debugNames[i]}, no longer raises errors, but should be fixed (edit: cannot be fixed, won't be fixed, not a problem (I hope))");
                continue;
            }

            prb.rb.constraints = RigidbodyConstraints.None;
            prb.rb.velocity = prb.velocity;
            prb.rb.angularVelocity = prb.angularVelocity;
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