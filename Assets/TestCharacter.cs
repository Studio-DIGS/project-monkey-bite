using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCharacter : BasicCharacter
{
    float gravity = -10f;
    public float moveSpeed = 10f;
    public float acceleration = 5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        movement.x = input.x * moveSpeed;
        // if (Input.GetKeyDown("space"))
        // {
        //     movement.y += 
        // }
        movement.y += gravity * Time.deltaTime;
        controller.Move(movement * Time.deltaTime);
    }
}
