using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    private float damage;
    private float speed;
    private float lifetime = 5.0f;
    private float rotationSpeed = 100.0f;
    private float beforeCleanupDuration = 2.0f;

    private GameObject target;
    private Rigidbody rb;
    private BoxCollider boxCollider;
    private MoveTowards moveTowards;
    private GameManager gameManager;

    public float Damage { get => damage; set => damage = value; }
    public float Speed { get => speed; set {
            speed = value;
            
            if (moveTowards != null) {
                moveTowards.Speed = value;
            }
        }}

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        target = FindClosestEnemy();
        moveTowards = GetComponent<MoveTowards>();
        moveTowards.Target = target;
        moveTowards.Speed = Speed;
        moveTowards.RotateTowardsTarget = false;

        Invoke(nameof(DisableProjectile), lifetime);
    }

    private void Update() {
        if (target == null) {
            CancelInvoke(nameof(DisableProjectile));
            DisableProjectile();

            return;
        }

        RotateProjectile();
    }

    void DisableProjectile() {
        boxCollider.isTrigger = false;
        rb.useGravity = true;
        moveTowards.MovementEnabled = false;

        Invoke(nameof(DestroyProjectile), beforeCleanupDuration);
    }

    void DestroyProjectile() {
        Destroy(gameObject);
    }


    void RotateProjectile() {
        transform.Rotate(Vector3.up * Time.deltaTime * rotationSpeed);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject == target) {
            Alive targetAlive = target.GetComponent<Alive>();

            targetAlive.ReceiveDamage(Damage);

            Destroy(gameObject);
        }
    }

    GameObject FindClosestEnemy() {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (enemies.Length == 0) { return null; }

        Vector3 playerPosition = transform.position;
        GameObject closestEnemy = enemies[0];

        foreach (GameObject enemy in enemies) {
            Vector3 enemyPosition = enemy.transform.position;
            float distance = Vector3.Distance(playerPosition, enemyPosition);
            float closestEnemyDistance = Vector3.Distance(playerPosition, closestEnemy.transform.position);

            if (distance >= closestEnemyDistance) {
                continue;
            }

            closestEnemy = enemy;
        }

        return closestEnemy;
    }
}
