using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (PathController))]
public class BasicCharacter : MonoBehaviour
{
    [HideInInspector] public Vector2 movement;
    // Contains: 
        // Staggering system (hitboxes and hurtboxes)
        // Health System including death
        // etc

    public PathController controller;

    void Start()
    {
        controller = GetComponent<PathController>();
        
    }
}
