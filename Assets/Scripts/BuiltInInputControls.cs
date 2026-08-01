using UnityEngine;

/// <summary>
/// Centralized wrapper around Unity's built-in input API for gameplay controls.
/// </summary>
public static class BuiltInInputControls
{
    private static Vector3 previousMousePosition;
    private static int mousePositionFrame = -1;

    public static float GetThrottle()
    {
        float value = 0f;
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            value += 1f;
        }

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            value -= 1f;
        }

        return Mathf.Clamp(value, -1f, 1f);
    }

    public static float GetSteering()
    {
        float value = 0f;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            value += 1f;
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            value -= 1f;
        }

        return Mathf.Clamp(value, -1f, 1f);
    }

    public static Vector2 GetMouseDelta()
    {
        Vector3 currentMousePosition = Input.mousePosition;
        if (mousePositionFrame < 0)
        {
            previousMousePosition = currentMousePosition;
            mousePositionFrame = Time.frameCount;
            return Vector2.zero;
        }

        Vector2 delta = currentMousePosition - previousMousePosition;
        if (mousePositionFrame != Time.frameCount)
        {
            previousMousePosition = currentMousePosition;
            mousePositionFrame = Time.frameCount;
        }

        return delta;
    }

    public static bool IsBrakePressed() => Input.GetKey(KeyCode.Space);
    public static bool IsAltPressed() => Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt);
    public static bool WasEngineTogglePressed() => Input.GetKeyDown(KeyCode.E);
    public static bool WasCameraCyclePressed() => Input.GetKeyDown(KeyCode.C);
    public static bool WasEscapePressed() => Input.GetKeyDown(KeyCode.Escape);

    public static bool WasNumberPressed(int number)
    {
        return number switch
        {
            1 => Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1),
            2 => Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2),
            3 => Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3),
            4 => Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4),
            _ => false
        };
    }
}
