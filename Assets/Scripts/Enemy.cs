using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject deathEffect;

    private void OnParticleCollision(GameObject other)
    {
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}