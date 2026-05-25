using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public static event System.Action OnA_ButtonPressed;
    public static event System.Action OnB_ButtonPressed;
    public static event System.Action OnC_ButtonPressed;
    public static event System.Action OnD_ButtonPressed;

    public void OnButtonA (InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnA_ButtonPressed?.Invoke();
        }
    }

    public void OnButtonB (InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnB_ButtonPressed?.Invoke();
        }
    }

    public void OnButtonC (InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnC_ButtonPressed?.Invoke();
        }
    }

    public void OnButtonD (InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnD_ButtonPressed?.Invoke();
        }
    }
}
