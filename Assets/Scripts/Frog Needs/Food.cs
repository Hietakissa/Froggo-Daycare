using UnityEngine;

public class Food : MonoBehaviour
{
    [SerializeField] float nutrition;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Frog frog))
        {
            frog.stats.hungerStat.IncreaseStat(nutrition * Random.Range(0.9f, 1.2f));
            if (PlayerData.lastGrabObject == gameObject) GrabbingController.Instance.UnGrabObject();
            Destroy(gameObject);

            GameManager.EatFood();
        }
    }
}
