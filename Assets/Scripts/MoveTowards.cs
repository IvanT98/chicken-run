using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MoveTowards : MonoBehaviour {
    private bool movementEnabled = true;
    private bool rotateTowardsTarget = true;
    private float speed;
    private float rotationSpeed = 1.0f;
    private GameObject target;
    private GameManager gameManager;

    public bool MovementEnabled { get => movementEnabled; set => movementEnabled = value; }
    public float Speed { get => speed; set => speed = value; }
    public GameObject Target { get => target; set => target = value; }
    public bool RotateTowardsTarget { get => rotateTowardsTarget; set => rotateTowardsTarget = value; }

    private void Start() {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update() {
        if (Target == null || gameManager.isGameOver()) {
            return;
        }

        Move();
        Rotate();
    }

    void Move() {
        if (!MovementEnabled) { return; }

        Vector3 targetPosition = Target.transform.position;
        Vector3 normalizedDistance = (targetPosition - transform.position).normalized;

        transform.Translate(normalizedDistance * Time.deltaTime * Speed, Space.World);
    }

    void Rotate() {
        if (!RotateTowardsTarget || !MovementEnabled) { return; }

        Vector3 targetDirection = Target.transform.position - transform.position;
        float singleStep = rotationSpeed * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDirection);
    }
}
