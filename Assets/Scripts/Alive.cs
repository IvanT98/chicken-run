using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Alive : MonoBehaviour {
    public TextMeshProUGUI healthText;

    private bool alive = true;
    [SerializeField]
    private float health;
    private float maxHealth;

    void Start() {
        maxHealth = health;
        SetHealthText();
    }

    // Update is called once per frame
    void Update() {
        alive = health > 0;
    }

    public bool IsAlive() {
        return alive;
    }

    public void ReceiveDamage(float damage) {
        health = Math.Max(0, health - damage);
        SetHealthText();
    }

    public void Heal(float healAmount) {
        health = Math.Min(maxHealth, health + healAmount);
        SetHealthText();
    }

    private void SetHealthText() {
        if (healthText == null) {
            return;
        }

        healthText.text = "Health: " + Mathf.Round(health);
    }
}
