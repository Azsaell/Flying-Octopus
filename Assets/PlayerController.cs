using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    private Rigidbody rb;
    public float movementSpeed;
    private int numberOfJumps = 2;
    private int numberOfDashes = 1;

    private Camera camera;
    private CapsuleCollider collider;
    
    void Start() {
        rb = GetComponent<Rigidbody>();
        camera = GetComponentInChildren<Camera>();
        collider = GetComponent<CapsuleCollider>();
    }

    
    void Update() {
        HandlePlayerMovement();
        HandleHorizontalRotation();
        HandleVerticalRotation();
        HandleJump();
        HandleCrouch();
        if (numberOfDashes > 0 && Input.GetKeyDown(KeyCode.LeftShift)) {
            var dashDirection = rb.velocity;
            dashDirection.y = 0;
            dashDirection = dashDirection.normalized;
            rb.AddForce(dashDirection * 5, ForceMode.Impulse);
            numberOfDashes--;
        }
        
    }

    private void HandleCrouch() {
        if (Input.GetKey(KeyCode.LeftControl)) {
            collider.height = 1f;
            collider.center.Set(0, -0.5f, 0);
        }
        else {
            collider.height = 2f;
            collider.center.Set(0, 0f, 0);
        }
    }

    private void HandleJump() {
        if (numberOfJumps > 0 && Input.GetKeyDown(KeyCode.Space)) {
            rb.AddForce(Vector3.up * 5, ForceMode.VelocityChange);
            numberOfJumps--;
        }
    }

    private void HandleVerticalRotation() {
        var mouseVerticalRotation = Input.GetAxis("Mouse Y");
        var newRotation = camera.transform.localRotation.eulerAngles;
        newRotation.x -= mouseVerticalRotation*1.2f;
        camera.transform.localRotation = Quaternion.Euler(newRotation);
    }

    private void HandleHorizontalRotation() {
        var mouseHorizontalRotation = Input.GetAxis("Mouse X");
        var newRotation = transform.localRotation.eulerAngles;
        newRotation.y += mouseHorizontalRotation*1.2f;
        transform.localRotation = Quaternion.Euler(newRotation);
    }

    private void HandlePlayerMovement() {
        var userKeyboardInput = new Vector3(
            Input.GetAxis("Horizontal"),
            0,
            Input.GetAxis("Vertical")
        );
        var velocity = transform.rotation * userKeyboardInput * movementSpeed;

        velocity.y = rb.velocity.y;
        rb.velocity = velocity;
        
        
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Ground")) {
            numberOfJumps = 2;
            numberOfDashes = 1;
        }
    }
}