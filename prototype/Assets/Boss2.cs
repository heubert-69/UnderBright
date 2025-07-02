using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2 : MonoBehaviour
{
    [Header("Movement Boss Settings")]
    [SerializeField] private int health = 300;
    [SerializeField] private float movementSpeed = 8f;
    [SerializeField] private float evasionCooldown = 5f;
    [SerializeField] private float weakPointDamageMultiplier = 3f;
    [SerializeField] private Transform[] patrolPoints;
    
    private int currentPatrolIndex;
    private float nextEvasionTime;
    private bool isExposed = false;
    
    void Start()
    {
        StartCoroutine(PatrolRoutine());
    }
    
    IEnumerator PatrolRoutine()
    {
        while (true)
        {
            Vector3 target = patrolPoints[currentPatrolIndex].position;
            while (Vector3.Distance(transform.position, target) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, target, movementSpeed * Time.deltaTime);
                yield return null;
            }
            
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
            
            // Occasionally pause at points
            if (Random.value > 0.7f)
                yield return new WaitForSeconds(1.5f);
        }
    }
    
    void Update()
    {
        if (Time.time >= nextEvasionTime && !isExposed)
        {
            StartCoroutine(EvadeAttack());
            nextEvasionTime = Time.time + evasionCooldown;
        }
    }
    
    IEnumerator EvadeAttack()
    {
        // Teleport or dash to avoid damage
        float evadeDuration = 2f;
        float startTime = Time.time;
        
        while (Time.time < startTime + evadeDuration)
        {
            // Flash between positions or create afterimages
            transform.position += new Vector3(Random.Range(-2f, 2f), 0, Random.Range(-2f, 2f));
            yield return new WaitForSeconds(0.1f);
        }
        
        // Become vulnerable after evading
        isExposed = true;
        yield return new WaitForSeconds(1f);
        isExposed = false;
    }
    
    public void TakeDamage(int damage, bool isWeakPoint)
    {
        if (!isExposed && !isWeakPoint) return;
        
        health -= isWeakPoint ? (int)(damage * weakPointDamageMultiplier) : damage;
        
        if (health <= 0)
            Die();
    }
    
    private void Die()
    {
        StopAllCoroutines();
        // Death sequence
        Destroy(gameObject);
    }
}