using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public void OnMove(InputValue value)
    {
        Vector2 moveInput = value.Get<Vector2>();

        if (moveInput == Vector2.zero)
        {
            Debug.Log("Currently not moving");
            return;
        }

        if (moveInput.y > 0)
            Debug.Log("Currently going UP");

        if (moveInput.y < 0)
            Debug.Log("Currently going DOWN");

        if (moveInput.x > 0)
            Debug.Log("Currently going RIGHT");

        if (moveInput.x < 0)
            Debug.Log("Currently going LEFT");
    }
}