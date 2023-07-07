using UnityEngine;

[CreateAssetMenu(fileName = "Cannon", menuName = "ScriptableObjects/Cannons", order = 5)]
public class CannonSO : ScriptableObject
{
    [SerializeField]
    private float _reloadTime;
    [SerializeField]
    private float _fireDelayTime;

    public float GetReloadTime => _reloadTime;
    public float GetFireDelayTime => _fireDelayTime;
}
