using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private ParticleSystem deathEffect;

    private void OnParticleCollision(GameObject other)
    {
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(transform.gameObject);
    }
}