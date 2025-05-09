//EnemyMovement.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public enum State
    {
        Patrol,
        Chase
    }

    [Header("General Settings")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float chaseRadius = 5f;

    [Header("Patrol Settings")]
    [SerializeField] private Transform[] patrolPoints;
    private int currentPatrolIndex = 0;

    [Header("Chase Settings")]
    [SerializeField] private Transform playerTransform;

    private Rigidbody2D rb;
    private Animator animator;
    private State currentState;
    private Vector2 moveDirection;
    private Vector2 lastMoveDirection;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        if (playerTransform == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                playerTransform = player.transform;
        }

        currentState = State.Patrol;
    }

    private void Update()
    {
        if (playerTransform != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
            currentState = distanceToPlayer <= chaseRadius ? State.Chase : State.Patrol;
        }

        switch (currentState)
        {
            case State.Patrol:
                Patrol();
                break;
            case State.Chase:
                Chase();
                break;
        }

        Animate();
    }

    private void Patrol()
    {
        if (patrolPoints.Length == 0) return;

        Transform target = patrolPoints[currentPatrolIndex];
        moveDirection = (target.position - transform.position).normalized;
        rb.velocity = moveDirection * moveSpeed;

        if (Vector2.Distance(transform.position, target.position) < 0.2f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        }
    }

    private void Chase()
    {
        if (playerTransform == null) return;

        moveDirection = (playerTransform.position - transform.position).normalized;
        rb.velocity = moveDirection * moveSpeed;
    }

    private void Animate()
    {
        Vector2 velocity = rb.velocity;
        bool isWalking = velocity.magnitude > 0.05f;

        animator.SetBool("isWalking", isWalking);

        if (isWalking)
        {
            animator.SetFloat("InputX", velocity.x);
            animator.SetFloat("InputY", velocity.y);

            lastMoveDirection = velocity.normalized;
            animator.SetFloat("LastInputX", lastMoveDirection.x);
            animator.SetFloat("LastInputY", lastMoveDirection.y);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRadius);
    }
}