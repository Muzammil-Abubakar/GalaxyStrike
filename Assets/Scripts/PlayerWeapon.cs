
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] private RectTransform crosshair;

    // Drag your Player Ship / Player Rig here.
    // Its blue Z axis should point forward.
    [SerializeField] private Transform aimReference;

    // Maximum weapon movement from the ship's forward direction.
    [SerializeField] private float horizontalLimit = 70f;
    [SerializeField] private float verticalLimit = 30f;

    // How far the crosshair ray reaches when it hits nothing.
    [SerializeField] private float aimDistance = 1000f;

    private ParticleSystem[] weaponParticles;

    void Start()
    {
        Cursor.visible = false;
    }

    void Awake()
    {
        weaponParticles = GetComponentsInChildren<ParticleSystem>();

        foreach (ParticleSystem particle in weaponParticles)
        {
            var emission = particle.emission;
            emission.enabled = false;
        }
    }

    void Update()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();

        // Keep crosshair exactly on the mouse.
        crosshair.position = mousePosition;

        AimLasers();
    }

    private void AimLasers()
    {
        // --------------------------------------------------
        // 1. Find exactly where the crosshair is pointing.
        // --------------------------------------------------

        Ray ray = Camera.main.ScreenPointToRay(
            Mouse.current.position.ReadValue()
        );

        Vector3 targetPoint = ray.GetPoint(aimDistance);

        RaycastHit[] hits = Physics.RaycastAll(
            ray,
            aimDistance
        );

        // Find the closest valid hit that isn't our player.
        float closestDistance = float.MaxValue;

        foreach (RaycastHit hit in hits)
        {
            // Ignore anything belonging to the entire player hierarchy.
            if (hit.transform == transform ||
                hit.transform.IsChildOf(transform))
            {
                continue;
            }

            if (hit.distance < closestDistance)
            {
                closestDistance = hit.distance;
                targetPoint = hit.point;
            }
        }

        // --------------------------------------------------
        // 2. Calculate target direction from the ship.
        // --------------------------------------------------

        Vector3 targetDirection =
            targetPoint - aimReference.position;

        if (targetDirection.sqrMagnitude < 0.0001f)
        {
            return;
        }

        targetDirection.Normalize();

        // Convert world direction into the ship's local space.
        Vector3 localDirection =
            aimReference.InverseTransformDirection(targetDirection);

        // --------------------------------------------------
        // 3. Calculate LEFT / RIGHT angle.
        // --------------------------------------------------

        float horizontalAngle =
            Mathf.Atan2(
                localDirection.x,
                localDirection.z
            ) * Mathf.Rad2Deg;

        // --------------------------------------------------
        // 4. Calculate UP / DOWN angle.
        // --------------------------------------------------

        float horizontalDistance =
            Mathf.Sqrt(
                localDirection.x * localDirection.x +
                localDirection.z * localDirection.z
            );

        float verticalAngle =
            Mathf.Atan2(
                localDirection.y,
                horizontalDistance
            ) * Mathf.Rad2Deg;

        // --------------------------------------------------
        // 5. Clamp independently.
        // --------------------------------------------------

        horizontalAngle = Mathf.Clamp(
            horizontalAngle,
            -horizontalLimit,
            horizontalLimit
        );

        verticalAngle = Mathf.Clamp(
            verticalAngle,
            -verticalLimit,
            verticalLimit
        );

        // --------------------------------------------------
        // 6. Build the final constrained direction.
        // --------------------------------------------------

        Quaternion constrainedRotation =
            Quaternion.Euler(
                -verticalAngle,
                horizontalAngle,
                0f
            );

        Vector3 constrainedLocalDirection =
            constrainedRotation * Vector3.forward;

        // Convert back into world space.
        Vector3 constrainedWorldDirection =
            aimReference.TransformDirection(
                constrainedLocalDirection
            );

        // --------------------------------------------------
        // 7. Aim the lasers.
        // --------------------------------------------------

        foreach (ParticleSystem laser in weaponParticles)
        {
            laser.transform.rotation =
                Quaternion.LookRotation(
                    constrainedWorldDirection,
                    aimReference.up
                );
        }
    }

    public void OnFire(InputValue value)
    {
        bool isFiring = value.isPressed;

        foreach (ParticleSystem particle in weaponParticles)
        {
            var emission = particle.emission;
            emission.enabled = isFiring;
        }
    }
}

