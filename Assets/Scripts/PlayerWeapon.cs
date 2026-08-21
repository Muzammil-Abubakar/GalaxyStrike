using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] private RectTransform crosshair;

    // The invisible point that the lasers will aim toward.
    [SerializeField] private Transform targetPoint;

    // How far in front of the camera the target point should be.
    [SerializeField] private float targetDistance = 250f;

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

        // Move the target point to the mouse position in 3D space.
        MoveTargetPoint();
    }

    private void MoveTargetPoint()
    {
        // Create a screen-space position using:
        // X = mouse X
        // Y = mouse Y
        // Z = distance in front of the camera
        Vector3 targetPointPosition = new Vector3(
            Mouse.current.position.ReadValue().x,
            Mouse.current.position.ReadValue().y,
            targetDistance
        );

        // Convert the screen position into a world position.
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(targetPointPosition);

        // Move our target point to that world position.
        targetPoint.position = worldPosition;
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