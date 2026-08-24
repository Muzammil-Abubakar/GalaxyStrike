
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Laser Death Time")]
    [Tooltip("Total continuous laser exposure required to kill the enemy, in milliseconds.")]
    [SerializeField] private float laserDeathTimeMilliseconds = 1000f;

    [Header("Laser Check")]
    [Tooltip("How often the script checks whether the laser is still hitting the enemy.")]
    [SerializeField] private float laserCheckInterval = 0.05f;

    [Header("Death")]
    [SerializeField] private GameObject deathEffect;

    [Header("Debug")]
    [SerializeField] private bool enableDebugLogs = true;

    private float accumulatedLaserTime;
    private float laserCheckTimer;

    private bool laserHitThisInterval;
    private bool isDead;

    private void Awake()
    {
        laserDeathTimeMilliseconds =
            Mathf.Max(laserDeathTimeMilliseconds, 0f);

        laserCheckInterval =
            Mathf.Max(laserCheckInterval, 0.001f);

        if (enableDebugLogs)
        {
            Debug.Log(
                $"[Enemy] {gameObject.name} initialized.\n" +
                $"Laser Death Time: {laserDeathTimeMilliseconds:F2} ms\n" +
                $"Laser Check Interval: {laserCheckInterval:F3}s",
                this
            );
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        if (isDead)
        {
            return;
        }

        // A particle collision means the laser has hit
        // the enemy during the current check interval.
        laserHitThisInterval = true;

        if (enableDebugLogs)
        {
            Debug.Log(
                $"[Enemy] {gameObject.name} received laser collision from: " +
                $"{other.name}",
                this
            );
        }
    }

    private void Update()
    {
        if (isDead)
        {
            return;
        }

        laserCheckTimer += Time.deltaTime;

        // Do not run the laser check every frame.
        if (laserCheckTimer < laserCheckInterval)
        {
            return;
        }

        // Reset the timer while preserving any extra elapsed time.
        laserCheckTimer -= laserCheckInterval;

        CheckLaserExposure();
    }

    private void CheckLaserExposure()
    {
        // Laser is currently hitting the enemy.
        if (laserHitThisInterval)
        {
            float checkTime = laserCheckInterval;

            accumulatedLaserTime += checkTime;

            if (enableDebugLogs)
            {
                Debug.Log(
                    $"[Enemy] {gameObject.name} laser active. " +
                    $"Exposure: {accumulatedLaserTime:F2}s / " +
                    $"{laserDeathTimeMilliseconds / 1000f:F2}s",
                    this
                );
            }

            // Clear the flag so the next interval must
            // receive another collision to continue.
            laserHitThisInterval = false;

            // Check whether the required exposure time
            // has been reached.
            if (accumulatedLaserTime >= laserDeathTimeMilliseconds / 1000f)
            {
                Die();
            }
        }
        else
        {
            // Laser stopped hitting the enemy.
            // Pause the accumulated exposure time.
            if (accumulatedLaserTime > 0f && enableDebugLogs)
            {
                Debug.Log(
                    $"[Enemy] {gameObject.name} laser stopped. " +
                    $"Exposure timer paused at " +
                    $"{accumulatedLaserTime:F2}s.",
                    this
                );
            }
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
                $"[Enemy] {gameObject.name} DIED after " +
                $"{accumulatedLaserTime:F2}s of laser exposure.",
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
