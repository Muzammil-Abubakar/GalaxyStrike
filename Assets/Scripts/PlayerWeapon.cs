using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeapon : MonoBehaviour
{
    private ParticleSystem[] weaponParticles;

    void Awake()
    {
        // Find all Particle Systems under this object.
        weaponParticles = GetComponentsInChildren<ParticleSystem>();

        // Turn both laser emissions off initially.
        foreach (ParticleSystem particle in weaponParticles)
        {
            var emission = particle.emission;
            emission.enabled = false;
        }
    }

    public void OnFire(InputValue value)
    {
        bool isFiring = value.isPressed;

        // Toggle emission for both lasers.
        foreach (ParticleSystem particle in weaponParticles)
        {
            var emission = particle.emission;
            emission.enabled = isFiring;
        }
    }
}