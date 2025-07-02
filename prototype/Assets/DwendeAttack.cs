using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dwende : MonoBehaviour
{
    [Header("Chase Settings")]
    [SerializeField] private float chaseSpeed = 4f;
    [SerializeField] private float chaseRange = 10f; // Distance to start chasing
    [SerializeField] private float attackRange = 1.5f; // Distance to start attacking

    [Header("Slash Attack Settings")]
    [SerializeField] private float slashDamage = 10f;
    [SerializeField] private float slashCooldown = 1f;
    [SerializeField] private float knockbackForce = 5f;

    [Header("References")]
    [SerializeField] private Transform playerTarget;
    [SerializeField] private Animator anim;
    [SerializeField] private LayerMask playerLayer;

    private Rigidbody2D rb;
    private bool isFacingRight = true;
    private float lastSlashTime;
    private bool isAttacking = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (playerTarget == null)
            playerTarget = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (isAttacking) return; // Pause movement during attack

        float distanceToPlayer = Vector2.Distance(transform.position, playerTarget.position);

        // Chase Logic
        if (distanceToPlayer <= chaseRange && distanceToPlayer > attackRange)
        {
            ChasePlayer();
            anim.SetInteger("Mode", 1); // Chase animation
        }
        // Attack Logic
        else if (distanceToPlayer <= attackRange)
        {
            if (Time.time >= lastSlashTime + slashCooldown)
            {
                PerformSlash();
                anim.SetInteger("Mode", 2); // Attack animation
            }
            else
            {
                anim.SetInteger("Mode", 0); // Idle (waiting for cooldown)
            }
        }
        // Idle if player is out of range
        else
        {
            anim.SetInteger("Mode", 0); // Idle animation
            rb.velocity = Vector2.zero;
        }
    }
    //Imported from The General Algorithm i made from EnemyChase&Attack.cs
    private void ChasePlayer()
    {
        FlipTowardsPlayer();
        Vector2 direction = (playerTarget.position - transform.position).normalized;
        rb.velocity = new Vector2(direction.x * chaseSpeed, rb.velocity.y);
    }

    private void FlipTowardsPlayer()
    {
        if (playerTarget.position.x > transform.position.x && !isFacingRight)
            Flip();
        else if (playerTarget.position.x < transform.position.x && isFacingRight)
            Flip();
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
    //If the Player is near, The slash animation and attack gets activated 
    private void PerformSlash()
    {
        isAttacking = true;
        lastSlashTime = Time.time;

        // Detect player hit (2D OverlapCircle)
        Collider2D hitPlayer = Physics2D.OverlapCircle(transform.position, attackRange, playerLayer);
        if (hitPlayer != null)
        {
            hitPlayer.GetComponent<PlayerHealth>().TakeDamage(slashDamage);
            
            // Knockback
            Rigidbody2D playerRb = hitPlayer.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                int direction = isFacingRight ? 1 : -1;
                playerRb.velocity = new Vector2(direction * knockbackForce, playerRb.velocity.y);
            }
        }

        // Reset attack state after animation
        Invoke(nameof(ResetAttack), 0.5f); // Match this with animation length
    }

    private void ResetAttack()
    {
        isAttacking = false;
    }

    // Visualize ranges in editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}