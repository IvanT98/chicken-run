using UnityEngine;
using TMPro;
using UnityEngine.UIElements;

public class PlayerPowerup : MonoBehaviour
{
    public TextMeshProUGUI powerupCountdownText;

    private float attackSpeedM = 1.0f;
    private float attackDamageM = 1.0f;
    private float attackQuantityM = 1.0f;
    private float powerupCountdown;

    public void ApplyPowerup(float powerupDuration) {
        CancelInvoke(nameof(ResetPowerup));

        this.attackSpeedM = projectileSpeedModifier;
        this.attackDamageM = projectileDamageModifier;
        this.attackQuantityM = projectileAmountModifier;

        powerupCountdown = powerupDuration;
        powerupCountdownText.gameObject.SetActive(true);

        Invoke(nameof(ResetPowerup), powerupDuration);
    }

    // Update is called once per frame
    void Update()
    {
        CountdownPowerup();
    }

    void ResetPowerup() {
        attackSpeedM = 1.0f;
        projectileSpawnSpeedModifier = 1.0f;
        attackDamageM = 1.0f;
        attackQuantityM = 1.0f;
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
