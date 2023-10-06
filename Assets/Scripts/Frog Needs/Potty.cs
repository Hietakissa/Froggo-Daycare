using UnityEngine;

public class Potty : MonoBehaviour
{
    [SerializeField] Transform pottyPosition;

    Frog lastFrog;

    float lastFrogThrowTime = -10f;

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Frog frog) && lastFrogThrowTime + 5f < Time.time)
        {
            if (lastFrog != null && lastFrog.StateIs(FrogState.Potty)) return;

            lastFrog = frog;
            frog.overridePosition = pottyPosition;
            frog.EnterState(FrogState.Potty);
        }
    }

    void Update()
    {
        if (lastFrog != null && lastFrog.StateIs(FrogState.Potty))
        {
            //last frog is on the potty
            if (lastFrog.stats.toiletStat.GetStatValue() == 100)
            {
                lastFrog.EnterState(FrogState.Roaming);

                float horizontalForce = 2f;
                float verticalForce = 7f;
                Vector3 randomForce = new Vector3(Random.Range(-horizontalForce, horizontalForce), verticalForce, Random.Range(-horizontalForce, horizontalForce));
                lastFrog.rb.AddForce(randomForce, ForceMode.Impulse);

                lastFrogThrowTime = Time.time;
            }
        }
    }
}
