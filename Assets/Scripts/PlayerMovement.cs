using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    [Header("Rotation")]
    [SerializeField] private float rollAngle = 15f;
    [SerializeField] private float pitchAngle = 25f;
    [SerializeField] private float rollRotationSpeed = 8f;
    [SerializeField] private float pitchRotationSpeed = 12f;

    private Vector2 moveInput;

    private void Update()
    {
        ProcessTranslation();
        ProcessRotation();
    }

    private void ProcessTranslation()
    {
        Vector3 localPosition = transform.localPosition;

        localPosition.x += moveInput.x * moveSpeed * Time.deltaTime;
        localPosition.y -= moveInput.y * moveSpeed * Time.deltaTime;

        localPosition.x = Mathf.Clamp(localPosition.x, -20f, 20f);
        localPosition.y = Mathf.Clamp(localPosition.y, -13f, 13f);

        transform.localPosition = localPosition;
    }

    private void ProcessRotation()
    {
        float targetZ = -moveInput.x * rollAngle;
        float targetX = moveInput.y * pitchAngle;

        Vector3 currentEuler = transform.localEulerAngles;

        float currentX = NormalizeAngle(currentEuler.x);
        float currentZ = NormalizeAngle(currentEuler.z);

        float newX = Mathf.Lerp(
            currentX,
            targetX,
            pitchRotationSpeed * Time.deltaTime
        );

        float newZ = Mathf.Lerp(
            currentZ,
            targetZ,
            rollRotationSpeed * Time.deltaTime
        );

        transform.localRotation = Quaternion.Euler(newX, 0f, newZ);
    }

    private float NormalizeAngle(float angle)
    {
        if (angle > 180f)
            angle -= 360f;

        return angle;
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }
}