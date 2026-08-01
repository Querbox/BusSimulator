using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Centralized wrapper around Unity's Input System package for gameplay controls.
/// </summary>
public static class InputSystemControls
{
    public static float GetThrottle()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null)
        {
            return 0f;
        }

        float value = 0f;
        if (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed)
        {
            value += 1f;
        }

        if (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed)
        {
            value -= 1f;
        }

        return Mathf.Clamp(value, -1f, 1f);
    }

    public static float GetSteering()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null)
        {
            return 0f;
        }

        float value = 0f;
        if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed)
        {
            value += 1f;
        }

        if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed)
        {
            value -= 1f;
        }

        return Mathf.Clamp(value, -1f, 1f);
    }

    public static Vector2 GetMouseDelta()
    {
        var mouse = Mouse.current;
        return mouse == null ? Vector2.zero : mouse.delta.ReadValue();
    }

    public static bool IsBrakePressed() => Keyboard.current?.spaceKey.isPressed == true;
    public static bool IsAltPressed() => Keyboard.current?.leftAltKey.isPressed == true || Keyboard.current?.rightAltKey.isPressed == true;
    public static bool WasEngineTogglePressed() => Keyboard.current?.eKey.wasPressedThisFrame == true;
    public static bool WasCameraCyclePressed() => Keyboard.current?.cKey.wasPressedThisFrame == true;
    public static bool WasEscapePressed() => Keyboard.current?.escapeKey.wasPressedThisFrame == true;

    public static bool WasNumberPressed(int number)
    {
        var keyboard = Keyboard.current;
        if (keyboard == null)
        {
            return false;
        }

        return number switch
        {
            1 => keyboard.digit1Key.wasPressedThisFrame || keyboard.numpad1Key.wasPressedThisFrame,
            2 => keyboard.digit2Key.wasPressedThisFrame || keyboard.numpad2Key.wasPressedThisFrame,
            3 => keyboard.digit3Key.wasPressedThisFrame || keyboard.numpad3Key.wasPressedThisFrame,
            4 => keyboard.digit4Key.wasPressedThisFrame || keyboard.numpad4Key.wasPressedThisFrame,
            _ => false
        };
    }
}
