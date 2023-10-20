using UnityEngine;

[CreateAssetMenu(menuName = "Froggo Daycare/Hat")]
public class HatSO : ScriptableObject
{
    public GameObject Prefab;
    public int ID;
    public Vector3 offset;
    public Vector3 rotation;
    public Vector3 scale;
}
