using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    private const int INPUT_QUEUE_SIZE = 3;

    //-1, 0, or 1. TODO: Refactor into an enum
    public float Move { get; private set; }
    public float LastMoveValue { get; set; }
    public bool GrappleHeld { get; private set; }
    public bool AttackHeld { get; private set; }
    public bool JumpHeld { get; private set; }

    private List<InputObject> inputObjects = new List<InputObject>(INPUT_QUEUE_SIZE);
    private bool rightMoveHeld = false;
    private bool leftMoveHeld = false;

    [Space(15)]
    [Header("Debug")]
    public List<InputObject> DEBUG_inputObjects;
    public bool DEBUG_ReadCurrentInput;
    public float DEBUG_move;
    public bool DEBUG_grappleHeld;
    public bool DEBUG_attackHeld;
    public float DEBUG_lastMoveValue;

    private void Update()
    {
        RemoveInvalidInputs(true);

        DEBUG_inputObjects = inputObjects;

        if (DEBUG_ReadCurrentInput)
        {
            DEBUG_ReadCurrentInput = false;
            Debug.Log(ReadCurrentInput());
        }
        DEBUG_move = Move;
        DEBUG_attackHeld = AttackHeld;
        DEBUG_grappleHeld = GrappleHeld;
        DEBUG_lastMoveValue = LastMoveValue;
    }

    public InputObject ReadCurrentInput()
    {
        RemoveInvalidInputs();

        InputObject currentInput = null;

        if(inputObjects.Count > 0)
        {
            currentInput = inputObjects[0];
        }

        if (currentInput != null)
        {
            currentInput.MarkAsRead();
        }

        return currentInput;
    }

    private void RemoveInvalidInputs(bool removeReadInputs = false)
    {
        int i = 0;
        while (i < inputObjects.Count)
        {
            InputObject inputObject = inputObjects[i];

            if (inputObject == null)
            {
                throw new System.Exception("There was a null value inserted into the inputObjects queue. Make sure only valid InputObjects are inserted into the queue.");
            }

            if (
                    Time.time >= inputObject.ExpiresOn ||
                    (removeReadInputs && inputObject.HasBeenRead)
               )
            {
                inputObjects.RemoveAt(i);
                continue;
            }

            i++;
        }
    }

    public void RegisterInput(InputObject newIo)
    {
        if (newIo == null || inputObjects.Count >= INPUT_QUEUE_SIZE)
        {
            return;
        }

        inputObjects.Add(newIo);
    }

    public void HandleLeftInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OverrideMoveDirection(-1);
            leftMoveHeld = true;
        }
        else if (context.canceled)
        {
            LastMoveValue = Move;
            leftMoveHeld = false;

            DetermineMoveDirection();
        }
    }

    public void HandleRightInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OverrideMoveDirection(1);
            rightMoveHeld = true;

        }
        else if (context.canceled)
        {
            LastMoveValue = Move;
            rightMoveHeld = false;

            DetermineMoveDirection();
        }
    }

    private void OverrideMoveDirection(float newDirection)
    {
        Move = newDirection;
    }
    private void DetermineMoveDirection()
    {
        Move = 0;

        if (rightMoveHeld)
        {
            Move = 1;
        }
        else if (leftMoveHeld)
        {
            Move = -1;
        }
    }

    public void HandleJumpInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            JumpHeld = true;

            InputObject newIO = new InputObject(InputType.Jump);
            RegisterInput(newIO);
        }
        else
        {
            JumpHeld = false;
        }
    }

    public void HandleGrappleInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            GrappleHeld = true;

            InputObject newIO = new InputObject(InputType.Grapple);
            RegisterInput(newIO);
        }
        else if (context.canceled)
        {
            GrappleHeld = false;
        }
    }

    public void HandleAttackInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            AttackHeld = true;

            InputObject newIO = new InputObject(InputType.Attack);
            RegisterInput(newIO);
        }
        else if (context.canceled)
        {
            AttackHeld = false;
        }
    }

    public void HandleDodgeInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            InputObject newIO = new InputObject(InputType.Dodge);
            RegisterInput(newIO);
        }
    }
}
