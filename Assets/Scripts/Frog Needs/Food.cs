using UnityEngine;

public class Food : MonoBehaviour
{
    [SerializeField] float nutrition;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Frog frog))
        {
            frog.stats.hungerStat.IncreaseStat(nutrition);
            if (PlayerData.lastGrabObject == gameObject) GrabbingController.Instance.UnGrabObject();
            Destroy(gameObject);

            GameManager.EatFood();
        }
    }
}
