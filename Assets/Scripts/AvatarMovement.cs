using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarMovement : MonoBehaviour
{
    float rotationSpeed = 500f; // Adjust the rotation speed as needed
  
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void rotateLeft()
    {
        RotateObject(Vector3.up); // Rotate left
    }


    public void rotateRight()
    {
        RotateObject(-Vector3.up); // Rotate right
    }

    void RotateObject(Vector3 direction)
    {
        // Perform the rotation based on the given direction and speed
        transform.Rotate(direction * rotationSpeed * Time.deltaTime);
    }
}
