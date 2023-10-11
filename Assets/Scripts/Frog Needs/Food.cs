using UnityEngine;

public class Food : MonoBehaviour
{
    [SerializeField] float nutrition;

    void OnCollisionEnter(Collision collision)
    {
        if (GameManager.TryGetFrog(collision.collider, out Frog frog))
        {
            frog.stats.hungerStat.IncreaseStat(nutrition * Random.Range(0.9f, 1.2f));
            if (PlayerData.lastGrabObject == gameObject)
            {
                Debug.Log("Food ungrabbed object");
                GrabbingController.Instance.UnGrabObject();
            }
            Destroy(gameObject);

            GameManager.EatFood();
        }
    }
}
