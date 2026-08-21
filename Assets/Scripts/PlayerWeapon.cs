using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] private RectTransform crosshair;

    private ParticleSystem[] weaponParticles;

    void Start()
    {
        Cursor.visible = false;
    }

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

    void Update()
    {
        // Get mouse position in screen space.
        Vector2 mousePosition = Mouse.current.position.ReadValue();

        // Move the crosshair to the mouse position.
        crosshair.position = mousePosition;
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