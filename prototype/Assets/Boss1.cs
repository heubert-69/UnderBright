using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1 : MonoBehaviour
{
    [Header("Strength Boss Settings")]
    [SerializeField] private int health = 500;
    [SerializeField] private float attackDamage = 40f;
    [SerializeField] private float heavyAttackMultiplier = 2.5f;
    [SerializeField] private float attackCooldown = 3f;
    [SerializeField] private float enrageThreshold = 0.3f; // 30% health
    
    private float nextAttackTime;
    private bool isEnraged = false;
    
    void Update()
    {
        if (Time.time >= nextAttackTime)
        {
            if (health <= health * enrageThreshold && !isEnraged)
            {
                Enrage();
            }
            
            PerformAttack();
            nextAttackTime = Time.time + attackCooldown;
        }
    }
    
    private void PerformAttack()
    {
        if (isEnraged)
        {
            HeavySlamAttack();
            GroundPoundShockwave();
        }

        else
        {
            BasicSwingAttack();
        }
            
    }
    
    private void BasicSwingAttack()
    {
        // Melee attack in front
        Collider[] hitPlayers = Physics.OverlapBox(transform.position + transform.forward * 3f, 
            new Vector3(4, 4, 2), transform.rotation, LayerMask.GetMask("Player"));
        
        foreach (var player in hitPlayers)
        {
            player.GetComponent<PlayerHealth>().TakeDamage(attackDamage);
        }
    }
    
    private void HeavySlamAttack()
    {
        // Powerful area attack
        Collider[] hitPlayers = Physics.OverlapSphere(transform.position, 8f);
        
        foreach (var player in hitPlayers)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            float damage = attackDamage * heavyAttackMultiplier * (1 - distance/8f);
            player.GetComponent<PlayerHealth>().TakeDamage(damage);
        }
    }
    
    private void GroundPoundShockwave()
    {
        // Create shockwave effect that radiates outward
        // Could use Unity's physics or a particle system (needs Changing)
    }
    
    private void Enrage()
    {
        isEnraged = true;
        attackCooldown *= 0.7f; // Attack faster
        // Visual effects for enrage state
    }
    
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
            Die();
    }
    
    private void Die()
    {
        // Death animation and cleanup
        Destroy(gameObject);
    }
}