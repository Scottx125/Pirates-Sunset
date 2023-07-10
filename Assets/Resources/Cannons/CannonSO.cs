using UnityEngine;

[CreateAssetMenu(fileName = "Cannon", menuName = "ScriptableObjects/Cannons", order = 5)]
public class CannonSO : ScriptableObject
{
    [SerializeField]
    private float _reloadTime;
    [SerializeField]
    private float _maxFireDelayTime;
    [SerializeField]
    private float _minFireDelayTime;

    public float GetReloadTime => _reloadTime;
    public float GetMaxFireDelayTime => _maxFireDelayTime;
    public float GetMinFireDelayTime => _minFireDelayTime;
}
