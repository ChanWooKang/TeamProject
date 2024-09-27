using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class PlayerAssetsInputs : MonoBehaviour
{
    [Header("Character Input Values")]
    public Vector2 move;
    public Vector2 look;
    public bool jump;
    public bool sprint;
    public bool fire;
    public bool interact;
    public bool inventory;
    public bool aim;
    public bool craft;
    public float scrollY;
    public bool throws;
    public bool reload;

    [Header("Movement Settings")]
    public bool analogMovement;

    [Header("Mouse Cursor Settings")]
    public bool cursorLocked = true;
    public bool cursorInputForLock = true;

#if ENABLE_INPUT_SYSTEM
    public void OnMove(InputValue value)
    {        
        MoveInput(value.Get<Vector2>());
    }

    public void OnLook(InputValue value)
    {
        if (cursorInputForLock)
        {
            LookInput(value.Get<Vector2>());
        }            
    }

    public void OnJump(InputValue value)
    {
        JumpInput(value.isPressed);
    }

    public void OnSprint(InputValue value)
    {
        SprintInput(value.isPressed);
    }

    public void OnInteraction(InputValue value)
    {
        InteractionInput(value.isPressed);
    }

    public void OnInventory(InputValue value)
    {
        InventoryInput(value.isPressed);
    }

    public void OnAim(InputValue value)
    {
        AimInput(value.isPressed);
    }

    public void OnCraft(InputValue value)
    {
        CraftInput(value.isPressed);
    }

    public void OnFire(InputValue value)
    {
        FireInput(value.isPressed);
    }

    public void OnWeaponSelect(InputValue value)
    {
        WeaponSelectInput(value.Get<float>());
    }

    public void OnThrow(InputValue value)
    {
        ThrowInput(value.isPressed);
    }

    public void OnReload(InputValue value)
    {
        ReloadInput(value.isPressed);
    }
#endif
    public void MoveInput(Vector2 newMoveDirection)
    {        
        move = newMoveDirection;
    }

    public void LookInput(Vector2 newLookDirection)
    {
        look = newLookDirection;
    }

    public void JumpInput(bool newJumpState)
    {
        jump = newJumpState;
    }

    public void SprintInput(bool newSprintState)
    {
        sprint = newSprintState;
    }

    public void InteractionInput(bool newInteractState)
    {
        interact = newInteractState;
    }

    public void InventoryInput(bool newInventoryState)
    {
        inventory = newInventoryState;
    }
    public void AimInput(bool newAimState)
    {
        aim = newAimState;
    }

    public void CraftInput(bool newCraftState)
    {
        craft = newCraftState;
    }

    public void FireInput(bool newFireState)
    {
        fire = newFireState;
    }

    public void WeaponSelectInput(float newSelectState)
    {
        scrollY = newSelectState;
    }

    public void ThrowInput(bool newThrowState)
    {
        throws = newThrowState;
    }

    public void ReloadInput(bool newReloadState)
    {
        reload = newReloadState;
    }
    private void OnApplicationFocus(bool hasFocus)
    {
        SetCursorState(cursorLocked);
    }

    private void SetCursorState(bool newState)
    {
        Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
    }
}
