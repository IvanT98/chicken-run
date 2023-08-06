using System;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public TextMeshProUGUI powerupCountdownText;

    [SerializeField]
    private float speed = 10.0f;
    [SerializeField]
    private float rotationSpeed = 30.0f;
    [SerializeField]
    private float xBound = 19.0f;
    [SerializeField]
    private float zBound = 24.0f;
    [SerializeField]
    private float projectileSpawnSpeed = 1.0f;
    [SerializeField]
    private int maxProjectiles = 3;
    private float projectileSpeed = 5.0f;
    private float projectileDamage = 5.0f;
    private float movementSpeedModifier = 1.0f;
    private float projectileSpeedModifier = 1.0f;
    private float projectileSpawnSpeedModifier = 1.0f;
    private float projectileDamageModifier = 1.0f;
    private float projectileAmountModifier = 1.0f;
    private float powerupCountdown;

    private Alive alive;
    [SerializeField]
    private GameObject projectile;
    [SerializeField]
    private Animator animator;
    private GameManager gameManager;

    public void ApplyPowerup(float healAmount, float movementSpeedModifier, float projectileSpeedModifier, float projectileSpawnSpeedModifier, float projectileDamageModifier, float projectileAmountModifier, float powerupDuration) {
        CancelInvoke(nameof(ResetPowerup));
        
        if (healAmount > 0) {
            alive.Heal(healAmount);
        }

        this.movementSpeedModifier = movementSpeedModifier;
        this.projectileSpeedModifier = projectileSpeedModifier;
        this.projectileSpawnSpeedModifier = projectileSpawnSpeedModifier;
        this.projectileDamageModifier = projectileDamageModifier;
        this.projectileAmountModifier = projectileAmountModifier;

        powerupCountdown = powerupDuration;
        powerupCountdownText.gameObject.SetActive(true);

        Invoke(nameof(ResetPowerup), powerupDuration);
    }

    private void Start() {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        alive = GetComponent<Alive>();

        Invoke(nameof(SpawnProjectile), projectileSpawnSpeed);
    }

    // Update is called once per frame
    void Update() {
        if (gameManager.isGameOver()) {
            return;
        }

        if (!alive.IsAlive()) {
            gameManager.GameOver();
            Destroy(gameObject);

            return;
        }

        CountdownPowerup();
        MovePlayer();
        ConstrainPlayerMovement();
    }

    void MovePlayer() {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        float movementSpeed = speed * movementSpeedModifier;
        float verticalMovement = Time.deltaTime * movementSpeed * verticalInput;

        animator.SetFloat("Speed_f", verticalMovement * 100);
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

    void SpawnProjectile() {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] projectiles = GameObject.FindGameObjectsWithTag("Projectile");
        int maxActiveProjectiles = (int)Math.Round(maxProjectiles * projectileAmountModifier);

        if (enemies.Length == 0 || projectiles.Length >= maxActiveProjectiles) {
            Invoke(nameof(SpawnProjectile), 0);

            return;
        }

        GameObject spawnedProjectile = Instantiate(this.projectile, transform.position, this.projectile.transform.rotation);
        Projectile projectile = spawnedProjectile.GetComponent<Projectile>();

        projectile.Speed = projectileSpeed * projectileSpeedModifier;
        projectile.Damage = projectileDamage * projectileDamageModifier;

        Invoke(nameof(SpawnProjectile), projectileSpawnSpeed * projectileSpawnSpeedModifier);
    }

    void ResetPowerup() {
        movementSpeedModifier = 1.0f;
        projectileSpeedModifier = 1.0f;
        projectileSpawnSpeedModifier = 1.0f;
        projectileDamageModifier = 1.0f;
        projectileAmountModifier = 1.0f;
    }

    void CountdownPowerup() {
        if (powerupCountdown <= 0) {
            powerupCountdownText.gameObject.SetActive(false);

            return;
        }

        powerupCountdown -= Time.deltaTime;
        powerupCountdownText.text = "Powerup: " + Mathf.Round(powerupCountdown);
    }
}
