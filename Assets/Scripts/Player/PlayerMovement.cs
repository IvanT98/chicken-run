using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float MovementSpeedModifier { get => movementSpeedModifier; set => movementSpeedModifier = value; }

    [SerializeField]
    private float speed = 5.0f;
    [SerializeField]
    private float rotationSpeed = 100.0f;
    [SerializeField]
    private float xBound = 19.0f;
    [SerializeField]
    private float zBound = 24.0f;

    private float movementSpeedModifier = 1.0f;

    void Update()
    {
        MovePlayer();
        ConstrainPlayerMovement();
    }

    void MovePlayer() {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        float movementSpeed = speed * MovementSpeedModifier;
        float verticalMovement = Time.deltaTime * movementSpeed * verticalInput;

        // animator.SetFloat("Speed_f", verticalMovement * 100);
        // TODO: Inform animator that player is moving
        transform.Translate(Vector3.forward * verticalMovement);
        transform.Rotate(horizontalInput * rotationSpeed * Time.deltaTime * Vector3.up);
    }

    void ConstrainPlayerMovement() {
        if (transform.position.x < -xBound) {
            transform.position = new Vector3(-xBound, transform.position.y, transform.position.z);
        } else if (transform.position.x > xBound) {
            transform.position = new Vector3(xBound, transform.position.y, transform.position.z);
        }

        if (transform.position.z < -zBound) {
            transform.position = new Vector3(transform.position.x, transform.position.y, -zBound);
        } else if (transform.transform.position.z > zBound) {
            transform.position = new Vector3(transform.position.x, transform.position.y, zBound);
        }
    }
}
