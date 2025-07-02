using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss3 : MonoBehaviour
{
    [Header("Mirror Boss Settings")]
    [SerializeField] private int baseHealth = 400;
    [SerializeField] private float mimicDelay = 1.5f;
    [SerializeField] private float statMultiplier = 1.2f;

    private PlayerController player;
    private Dictionary<string, float> playerStats = new Dictionary<string, float>();
    private Queue<PlayerAttack> attackQueue = new Queue<PlayerAttack>();
    private int currentHealth;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        currentHealth = baseHealth;
        StartCoroutine(MimicRoutine());
    }

    IEnumerator MimicRoutine()
    {
        while (true)
        {
            // Copy player stats with multiplier
            playerStats["Damage"] = player.GetDamage() * statMultiplier;
            playerStats["AttackSpeed"] = player.GetAttackSpeed() * (1 / statMultiplier);
            playerStats["MovementSpeed"] = player.GetMovementSpeed() * statMultiplier;

            // Execute queued attacks
            if (attackQueue.Count > 0)
            {
                PlayerAttack nextAttack = attackQueue.Dequeue();
                ExecuteCopiedAttack(nextAttack);
            }

            yield return new WaitForSeconds(mimicDelay);
        }
    }

    public void RecordPlayerAttack(PlayerAttack attack)
    {
        attackQueue.Enqueue(attack);
    }

    private void ExecuteCopiedAttack(PlayerAttack attack)
    {
        switch (attack.Type)
        {
            case AttackType.Melee:
                PerformMirrorMelee(attack.Direction);
                break;
            case AttackType.Ranged:
                PerformMirrorRanged(attack.Direction, attack.Position);
                break;
            case AttackType.Special:
                PerformMirrorSpecial(attack.SpecialType);
                break;
        }
    }

    private void PerformMirrorMelee(Vector3 direction)
    {
        // Perform a melee attack in the mirrored direction
        float damage = playerStats["Damage"];
        // Implement attack logic
    }

    private void PerformMirrorRanged(Vector3 direction, Vector3 position)
    {
        // Fire a projectile in the mirrored direction
        float damage = playerStats["Damage"];
        // Implement ranged logic
    }

    private void PerformMirrorSpecial(SpecialType specialType)
    {
        // Mirror the player's special ability
        // Would need special case handling for each ability
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        StopAllCoroutines();
        // Death sequence
        Destroy(gameObject);
    }
}

//Lots of changing but will commit, need to discuss this with the team.
//Needs a supporting class that gets the player's stats at that environment