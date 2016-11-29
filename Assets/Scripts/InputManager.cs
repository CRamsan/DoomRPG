using UnityEngine;

public class InputManager
{
    public enum CONTROLLER_ACTION
    {
        NONE,
        WALK_FORWARD,
        WALK_BACKWARDS,
        STRAFE_LEFT,
        STRAFE_RIGHT,
        ACTION,
        PAUSE,
        PASS
    }

    public enum CONTROLLER_ACTION_AXIS
    {
        WALK,
        STRAFE,
    }


    private enum CONTROLLER_ACTION_EVENT
    {
        DOWN, PRESSED, UP
    }

    // Axis input takes precedence over key input
    private static float HandleMultiInput(string axisName, string keyNamePositive, string keyNameNegative)
    {
        float axisValue = Input.GetAxis(axisName);
        float inputValue = Mathf.Abs(axisValue) > 0.5f ? ((axisValue < 0) ? -1 : 1) : 0;
        if (inputValue == 0)
        {
            inputValue = (Input.GetKey(keyNamePositive) ? 1 : 0) - (Input.GetKey(keyNameNegative) ? 1 : 0);
        }
        return inputValue;
    }

    public static float GetAxis(CONTROLLER_ACTION action)
    {
        float state = 0.0f;
        switch (action)
        {
            case CONTROLLER_ACTION.STRAFE_RIGHT:
            case CONTROLLER_ACTION.STRAFE_LEFT:
                state = HandleMultiInput("CONTROLLER_AXIS_1", "d", "a");
                if (action == CONTROLLER_ACTION.STRAFE_LEFT)
                    state *= -1;
                break;
            case CONTROLLER_ACTION.WALK_BACKWARDS:
            case CONTROLLER_ACTION.WALK_FORWARD:
                state = HandleMultiInput("CONTROLLER_AXIS_2", "w", "s");
                if (action == CONTROLLER_ACTION.WALK_BACKWARDS)
                    state *= -1;
                break;
        }

        return state;
    }

    public static float GetAxis(CONTROLLER_ACTION_AXIS action)
    {
        float state = 0.0f;
        switch (action)
        {
            case CONTROLLER_ACTION_AXIS.STRAFE:
                state = HandleMultiInput("CONTROLLER_AXIS_1", "d", "a");
                break;
            case CONTROLLER_ACTION_AXIS.WALK:
                state = HandleMultiInput("CONTROLLER_AXIS_2", "w", "s");
                break;
        }

        return state;
    }

    private static bool ActionHandler(CONTROLLER_ACTION actionInput, CONTROLLER_ACTION_EVENT actionEvent)
    {
        bool result = false;
        string input = "";

        if (actionEvent == CONTROLLER_ACTION_EVENT.DOWN)
        {
            if (Mathf.Abs(GetAxis(actionInput)) > 0.5f)
            {
                return true;
            }
        }

        switch (actionInput)
        {
            case CONTROLLER_ACTION.STRAFE_LEFT:
                input = "a";
                break;
            case CONTROLLER_ACTION.STRAFE_RIGHT:
                input = "d";
                break;
            case CONTROLLER_ACTION.WALK_FORWARD:
                input = "w";
                break;
            case CONTROLLER_ACTION.WALK_BACKWARDS:
                input = "s";
                break;
            case CONTROLLER_ACTION.ACTION:
                input = "space";
                break;
            case CONTROLLER_ACTION.PAUSE:
                input = "p";
                break;
            case CONTROLLER_ACTION.PASS:
                input = "left ctrl";
                break;
            default:
                throw new System.Exception();
        }        

        switch (actionEvent)
        {
            case CONTROLLER_ACTION_EVENT.DOWN:
                result = Input.GetKeyDown(input);
                break;
            case CONTROLLER_ACTION_EVENT.PRESSED:
                result = Input.GetKey(input);
                break;
            case CONTROLLER_ACTION_EVENT.UP:
                result = Input.GetKeyUp(input);
                break;
        }

        return result;
    }

    private static float ActionHandler(CONTROLLER_ACTION_AXIS actionInput, CONTROLLER_ACTION_EVENT actionEvent)
    {
        float value = GetAxis(actionInput);
        if (Mathf.Abs(value) > 0.5f)
        {
            return value;
        }
        return 0;
    }

    public static bool WasActionPressed(CONTROLLER_ACTION action)
    {
        return ActionHandler(action, CONTROLLER_ACTION_EVENT.DOWN);
    }

    public static bool IsActionPressed(CONTROLLER_ACTION action)
    {
        return ActionHandler(action, CONTROLLER_ACTION_EVENT.PRESSED);
    }

    public static bool WasActionReleased(CONTROLLER_ACTION action)
    {
        return ActionHandler(action, CONTROLLER_ACTION_EVENT.UP);
    }

    public static float ActionPressed(CONTROLLER_ACTION_AXIS action)
    {
        return ActionHandler(action, CONTROLLER_ACTION_EVENT.PRESSED);
    }
}