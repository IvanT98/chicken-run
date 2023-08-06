using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    private bool collidingWithPlayer = false;
    [SerializeField]
    private float damage = 5.0f;
    private float damageCooldown = 1.0f;
    private float minSpeed = 1.0f;
    private float maxSpeed = 3.0f;

    private GameObject player;
    private MoveTowards moveTowards;
    [SerializeField]
    private Animator animator;
    private Alive alive;

    // Start is called before the first frame update
    void Start() {
        alive = GetComponent<Alive>();
        player = GameObject.Find("Player");
        moveTowards = GetComponent<MoveTowards>();
        moveTowards.Target = player;
        moveTowards.Speed = Random.Range(minSpeed, maxSpeed);
        animator.SetFloat("Speed_f", moveTowards.Speed);
    }

    void Update() {
        if (!alive.IsAlive() || player == null) {
            Destroy(gameObject);
        }
    }

    void DamagePlayer() {
        if (!collidingWithPlayer || player == null) return;

        Alive playerAlive = player.GetComponent<Alive>();

        playerAlive.ReceiveDamage(damage);

        Invoke(nameof(DamagePlayer), damageCooldown);
    }

    private void OnCollisionEnter(Collision collision) {
        GameObject otherGameObject = collision.gameObject;

        if (otherGameObject.CompareTag("Player")) {
            collidingWithPlayer = true;
            moveTowards.MovementEnabled = !collidingWithPlayer;
            animator.SetFloat("Speed_f", 0);
            animator.SetBool("Eat_b", true);

            Invoke(nameof(DamagePlayer), 0);
        }
    }

    private void OnCollisionExit(Collision collision) {
        if (collision.gameObject.CompareTag("Player")) {
            collidingWithPlayer = false;
            moveTowards.MovementEnabled = !collidingWithPlayer;

            CancelInvoke(nameof(DamagePlayer));

            animator.SetFloat("Speed_f", moveTowards.Speed);
            animator.SetBool("Eat_b", false);
        }
    }
}
