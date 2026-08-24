using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float maxHealth = 100f;

    [Header("Laser Damage")]
    [SerializeField] private float damagePerTick = 10f;
    [SerializeField] private float damageTickInterval = 0.2f;

    [Header("Death")]
    [SerializeField] private GameObject deathEffect;

    [Header("Debug")]
    [SerializeField] private bool enableDebugLogs = true;

    private float currentHealth;
    private float nextDamageTime;
    private bool isDead;

    private void Awake()
    {
        currentHealth = maxHealth;

        if (enableDebugLogs)
        {
            Debug.Log(
                $"[Enemy] {gameObject.name} initialized. " +
                $"Health: {currentHealth}/{maxHealth}",
                this
            );
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        if (isDead)
        {
            if (enableDebugLogs)
            {
                Debug.Log(
                    $"[Enemy] {gameObject.name} received a particle collision, " +
                    $"but it is already dead. Ignoring damage.",
                    this
                );
            }

            return;
        }

        if (enableDebugLogs)
        {
            Debug.Log(
                $"[Enemy] {gameObject.name} detected particle collision from: {other.name}",
                this
            );
        }

        // Prevent damage from being applied every collision message.
        if (Time.time < nextDamageTime)
        {
            if (enableDebugLogs)
            {
                Debug.Log(
                    $"[Enemy] {gameObject.name} collision detected, " +
                    $"but damage is on cooldown. " +
                    $"Next damage in {nextDamageTime - Time.time:F2}s.",
                    this
                );
            }

            return;
        }

        nextDamageTime = Time.time + damageTickInterval;

        TakeDamage(damagePerTick);
    }

    private void TakeDamage(float damage)
    {
        if (isDead)
        {
            return;
        }

        currentHealth -= damage;

        // Prevent health from going below zero.
        currentHealth = Mathf.Max(currentHealth, 0f);

        if (enableDebugLogs)
        {
            Debug.Log(
                $"[Enemy] {gameObject.name} took {damage} damage. " +
                $"Health: {currentHealth}/{maxHealth}",
                this
            );
        }

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead)
        {
            return;
        }

        isDead = true;

        if (enableDebugLogs)
        {
            Debug.Log(
                $"[Enemy] {gameObject.name} DIED.",
                this
            );
        }

        if (deathEffect != null)
        {
            Instantiate(
                deathEffect,
                transform.position,
                Quaternion.identity
            );

            if (enableDebugLogs)
            {
                Debug.Log(
                    $"[Enemy] {gameObject.name} spawned death effect.",
                    this
                );
            }
        }
        else
        {
            Debug.LogWarning(
                $"[Enemy] {gameObject.name} has no deathEffect assigned.",
                this
            );
        }

        Destroy(gameObject);
    }
}