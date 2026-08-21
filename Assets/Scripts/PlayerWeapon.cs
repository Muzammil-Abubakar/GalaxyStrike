using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] private ParticleSystem weaponParticles;

    private ParticleSystem.EmissionModule emission;

    void Awake()
    {
        // Finds the Particle System anywhere underneath this object.
        if (weaponParticles == null)
        {
            weaponParticles = GetComponentInChildren<ParticleSystem>();
        }

        if (weaponParticles != null)
        {
            emission = weaponParticles.emission;
            emission.enabled = false;
        }
    }

    public void OnFire(InputValue value)
    {
        bool isFiring = value.isPressed;
        emission.enabled = isFiring;
    }
}