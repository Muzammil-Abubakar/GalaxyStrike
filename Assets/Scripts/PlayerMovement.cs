using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    private Vector2 moveInput;

    private void Update()
    {
        Vector3 localPosition = transform.localPosition;

        localPosition.x += moveInput.x * moveSpeed * Time.deltaTime;
        localPosition.y += moveInput.y * moveSpeed * Time.deltaTime;

        localPosition.x = Mathf.Clamp(localPosition.x, -20f, 20f);
        localPosition.y = Mathf.Clamp(localPosition.y, -13f, 13f);

        transform.localPosition = localPosition;
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }
}