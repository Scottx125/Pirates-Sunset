using UnityEngine;

[CreateAssetMenu(fileName = "Cannon", menuName = "ScriptableObjects/Cannons", order = 5)]
public class CannonSO : ScriptableObject
{
    [SerializeField]
    private float _minReloadTime;
    [SerializeField]
    private float _maxReloadTime;
    [SerializeField]
    private float _maxFireDelayTime;
    [SerializeField]
    private float _minFireDelayTime;

    public float GetMinReloadTime => _minReloadTime;
    public float GetMaxReloadTime => _maxReloadTime;
    public float GetMaxFireDelayTime => _maxFireDelayTime;
    public float GetMinFireDelayTime => _minFireDelayTime;
}
