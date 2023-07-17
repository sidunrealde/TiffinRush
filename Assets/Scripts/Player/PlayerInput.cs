using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public static PlayerInput Instance { get; private set; }

    public event EventHandler OnInteractAction;
    public event EventHandler OnDoTaskAction;
    public event EventHandler OnPauseAction;

    private PlayerInputActions playerInputActions;
    private void Awake()
    {
        Instance = this;

        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        playerInputActions.Player.Interact.performed += InteractPerformed;
        playerInputActions.Player.DoTask.performed += DoTaskperformed;
        playerInputActions.Player.Pause.performed += PausePerformed;
    }

    private void OnDestroy()
    {
        playerInputActions.Player.Interact.performed -= InteractPerformed;
        playerInputActions.Player.DoTask.performed -= DoTaskperformed;
        playerInputActions.Player.Pause.performed -= PausePerformed;

        playerInputActions.Dispose();
    }

    private void PausePerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPauseAction?.Invoke(this, EventArgs.Empty);
    }

    private void DoTaskperformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnDoTaskAction?.Invoke(this, EventArgs.Empty);
    }

    private void InteractPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        // Break if null
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized()
    {

        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();
       
        // normalize to have same speed diagonally
        inputVector = inputVector.normalized;

        return inputVector;
    }
}
