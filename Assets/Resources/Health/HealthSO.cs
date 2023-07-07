using UnityEngine;

[CreateAssetMenu(fileName = "Health", menuName = "ScriptableObjects/Health", order = 3)]
public class HealthSO : ScriptableObject
{
    [SerializeField]
    private int _maxHealth;
    public int MaxHealth => _maxHealth;

}
