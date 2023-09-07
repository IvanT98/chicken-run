using System;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    [SerializeField]
    private float projectileSpawnSpeed = 1.0f;
    [SerializeField]
    private int maxProjectiles = 3;
    private float projectileSpeed = 5.0f;
    private float projectileDamage = 5.0f;

    private Alive alive;
    [SerializeField]
    private GameObject projectile;
    [SerializeField]
    private Animator animator;
    private GameManager gameManager;

    private PlayerMovement playerMovement;

    public void ApplyPowerup(float healAmount, float movementSpeedModifier, float projectileSpeedModifier, float projectileSpawnSpeedModifier, float projectileDamageModifier, float projectileAmountModifier, float powerupDuration) {
        CancelInvoke(nameof(ResetPowerup));

        if (healAmount > 0) {
            alive.Heal(healAmount);
        }

        playerMovement.MovementSpeedModifier = movementSpeedModifier;
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
        playerMovement = GetComponent<PlayerMovement>();

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
}
