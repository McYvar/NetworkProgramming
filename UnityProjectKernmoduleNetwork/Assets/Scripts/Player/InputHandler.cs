using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public int vertical = 0;
    public int horizontal = 0;

    public bool isPressedForward;
    public int pressForwardValue;
    public Action pressForwardFirst;
    public Action pressForwardLast;
    private void OnPressForward()
    {
        isPressedForward = true;
        pressForwardValue = 1;
        vertical += 1;
        pressForwardFirst?.Invoke();
    }
    private void OnReleaseForward()
    {
        isPressedForward = false;
        pressForwardValue = 0;
        vertical -= 1;
        pressForwardLast?.Invoke();
    }

    public bool isPressedBackward;
    public int pressBackwardValue;
    public Action pressBackwardFirst;
    public Action pressBackwardLast;
    private void OnPressBackward()
    {
        isPressedBackward = true;
        pressBackwardValue = 1;
        vertical -= 1;
        pressBackwardFirst?.Invoke();
    }
    private void OnReleaseBackward()
    {
        isPressedBackward = false;
        pressBackwardValue = 0;
        vertical += 1;
        pressBackwardLast?.Invoke();
    }

    public bool isPressedLeft;
    public int pressLeftValue;
    public Action pressLeftFirst;
    public Action pressLeftLast;
    private void OnPressLeft()
    {
        isPressedLeft = true;
        pressLeftValue = 1;
        horizontal -= 1;
        pressLeftFirst?.Invoke();
    }
    private void OnReleaseLeft()
    {
        isPressedLeft = false;
        pressLeftValue = 0;
        horizontal += 1;
        pressLeftLast?.Invoke();
    }

    public bool isPressedRight;
    public int pressRightValue;
    public Action pressRightFirst;
    public Action pressRightLast;
    private void OnPressRight()
    {
        isPressedRight = true;
        pressRightValue = 1;
        horizontal += 1;
        pressRightFirst?.Invoke();
    }
    private void OnReleaseRight()
    {
        isPressedRight = false;
        pressRightValue = 0;
        horizontal -= 1;
        pressRightLast?.Invoke();
    }

    public bool isPressedJump;
    public int pressJumpValue;
    public Action pressJumpFirst;
    public Action pressJumpLast;
    private void OnPressJump()
    {
        isPressedJump = true;
        pressJumpValue = 1;
        pressJumpFirst?.Invoke();
    }
    private void OnReleaseJump()
    {
        isPressedJump = false;
        pressJumpValue = 0;
        pressJumpLast?.Invoke();
    }

    public bool isPressedAnyKey;
    public int pressAnyKeyValue;
    public Action pressAnyKeyFirst;
    public Action pressAnyKeyLast;
    private void OnPressAnyKey()
    {
        isPressedAnyKey = true;
        pressAnyKeyValue = 1;
        pressAnyKeyFirst?.Invoke();
    }
    private void OnReleaseAnyKey()
    {
        isPressedAnyKey = false;
        pressAnyKeyValue = 0;
        pressAnyKeyLast?.Invoke();
    }

    public bool isPressedMouseLeft;
    public int pressMouseLeftValue;
    public Action pressMouseLeftFirst;
    public Action pressMouseLeftLast;
    private void OnPressMouseLeft()
    {
        isPressedMouseLeft = true;
        pressMouseLeftValue = 1;
        pressMouseLeftFirst?.Invoke();
    }
    private void OnReleaseMouseLeft()
    {
        isPressedMouseLeft = false;
        pressMouseLeftValue = 0;
        pressMouseLeftLast?.Invoke();
    }

    public bool isPressedMouseRight;
    public int pressMouseRightValue;
    public Action pressMouseRightFirst;
    public Action pressMouseRightLast;
    private void OnPressMouseRight()
    {
        isPressedMouseRight = true;
        pressMouseRightValue = 0;
        pressMouseRightFirst?.Invoke();
    }
    private void OnReleaseMouseRight()
    {
        isPressedMouseRight = false;
        pressMouseRightValue = 0;
        pressMouseRightLast?.Invoke();
    }

    public Vector2 mouseDelta;
    private void OnEnable() => InputSystem.EnableDevice(Mouse.current);
    private void OnDisable() => InputSystem.DisableDevice(Mouse.current);
    private void Update() => mouseDelta = Mouse.current.delta.ReadValue();
}
