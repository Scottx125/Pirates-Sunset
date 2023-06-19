using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private float sailState;
    [SerializeField]
    private float sailStateChangeDelay = 1f;
    private float sailStateTimeSinceChanged;
    [SerializeField]
    private float maxSpeed;
    [SerializeField]
    private float maxTurnSpeed;

    private void Awake()
    {
        Setup();
    }

    private void Setup()
    {
        // Allow the player to fire immediately.
        sailStateTimeSinceChanged = 1f;
    }

    public void FixedUpdate(){
        SailStateTimer();
        Debug.Log(sailState);
        CalculateMovement();
    }

    private void SailStateTimer()
    {
        if (sailStateTimeSinceChanged >= sailStateChangeDelay){
            sailStateTimeSinceChanged = 1f;
            return;
        }
        sailStateTimeSinceChanged += Time.deltaTime;
    }

    private void CalculateMovement()
    {
        // Calculate forward movement based on current sailstate.
    }

    public void SailStateIncrease(){
        if (sailState < 1f && sailStateTimeSinceChanged >= sailStateChangeDelay){
            sailState += 0.25f;
            sailStateTimeSinceChanged = 0f; 
        }
    }
    public void SailStateDecrease(){
        if (sailState > 0f && sailStateTimeSinceChanged >= sailStateChangeDelay){
            sailState -= 0.25f;
            sailStateTimeSinceChanged = 0f; 
        }
    }
}
