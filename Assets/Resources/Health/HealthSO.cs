using UnityEngine;

[CreateAssetMenu(fileName = "Health", menuName = "ScriptableObjects/Health", order = 3)]
public class HealthSO : ScriptableObject
{
    [SerializeField]
    private int _maxHullHealth, _maxCrewHealth, _maxSailHealth;

    public int GetMaxHullHealth => _maxHullHealth;
    public int GetMaxCrewHealth => _maxCrewHealth;
    public int GetMaxSailHealth => _maxSailHealth;
}
