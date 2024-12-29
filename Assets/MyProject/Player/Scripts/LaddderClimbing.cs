using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaddderClimbing : MonoBehaviour
{
    public float climbSpeed = 3.0f;
    public CharacterController characterController;
    public float normalGravity = -9.8f; 

    private bool isClimbing = false;
    private Vector3 playerVelocity;

    private void Update()
    {
        if (isClimbing)
        {
            HandleClimbing();
        }
        else
        {
            ApplyGravity();
        }

        characterController.Move(playerVelocity * Time.deltaTime);
    }

    private void HandleClimbing()
    {
        playerVelocity.y = 0;

        float verticalInput = Input.GetAxis("Vertical");
        playerVelocity.y = verticalInput * climbSpeed;
    }

    private void ApplyGravity()
    {

        if (characterController.isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f; 
        }
        else
        {
            playerVelocity.y += normalGravity * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ladder"))
        {
            isClimbing = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ladder"))
        {
            isClimbing = false;
        }
    }
}
