using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityTracker : MonoBehaviour
{
    [SerializeField]
    private List<AbilityType> _activeAbilitiesList = new List<AbilityType>();

    // Add ability method and remove ability method.
    // Add ability will check to see if a type exists.
    // Remove ability will simply remove the current type.
}
