using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLocomotionInput : MonoBehaviour, PlayerControls.IPlayerActions
{
    
    public PlayerControls PlayerControls { get; private set; }
    public Vector2 MoveInput { get; private set; }
    public Vector2 AimInput { get; private set; }
    public bool FirePressed { get;  set; }
    public  bool MeleePressed { get; set; }
    
    
    private void OnEnable()
    {
        PlayerControls = new PlayerControls();
        PlayerControls.Player.Enable();
        PlayerControls.Player.SetCallbacks(this);
    }
    
    private void OnDisable()
    {
        PlayerControls.Player.Disable();
        PlayerControls.Player.RemoveCallbacks(this);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveInput = context.ReadValue<Vector2>();
        if (context.canceled)
        {
            MoveInput = Vector2.zero;
        }
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        AimInput = context.ReadValue<Vector2>();
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            FirePressed = true;
        }  
        else if (context.canceled)
        {
            FirePressed = false;
        }
    }
    
    public void OnMelee(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            MeleePressed = true;
        }  
        else if (context.canceled)
        {
            MeleePressed = false;
        }
    }
}
