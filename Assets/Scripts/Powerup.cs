using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float healAmount = 0.0f;
    [SerializeField]
    private float powerupDuration = 0.0f;
    [SerializeField]
    private float movementSpeedModifier = 1.0f;
    [SerializeField]
    private float projectileSpeedModifier = 1.0f;
    [SerializeField]
    private float projectileSpawnSpeedModifier = 1.0f;
    [SerializeField]
    private float projectileDamageModifier = 1.0f;
    [SerializeField]
    private float projectileAmountModifier = 1.0f;

    private float rotationSpeed = 20.0f;

    private void Update() {
        transform.Rotate(Vector3.up * Time.deltaTime * rotationSpeed);
    }

    private void OnTriggerEnter(Collider other) {
        GameObject otherGameObject = other.gameObject;

        if (otherGameObject.CompareTag("Player")) {
            PlayerController playerController = otherGameObject.GetComponent<PlayerController>();

            playerController.ApplyPowerup(healAmount, movementSpeedModifier, projectileSpeedModifier, projectileSpawnSpeedModifier, projectileDamageModifier, projectileAmountModifier, powerupDuration);

            Destroy(gameObject);
        }
    }
}
