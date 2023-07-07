using UnityEngine;

[CreateAssetMenu(fileName = "Ship", menuName = "ScriptableObjects/Ships", order = 1)]
public class ShipSO : ScriptableObject
{
    [SerializeField]
    private GameObject shipPrefab; 
    // Maybe have this as a list holding all the ships.

}
