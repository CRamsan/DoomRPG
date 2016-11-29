using UnityEngine;
using System.Collections;

public class MainPlayerController : PlayerController
{

    public GameObject cameraDelegate;
    public LoadingSpinnerController spinnerController;

    private bool isBusy;
    private bool isPaused;

    // Use this for initialization
    void Start()
    {
        isMoving = false;
    }

    protected new void FixedUpdate()
    {
        if (isBusy)
            return;

        if (!isMoving)
        {
            InputManager.CONTROLLER_ACTION action = InputManager.CONTROLLER_ACTION.NONE;
            if (InputManager.IsActionPressed(InputManager.CONTROLLER_ACTION.WALK_FORWARD))
            {
                action = InputManager.CONTROLLER_ACTION.WALK_FORWARD;
            }
            else if (InputManager.IsActionPressed(InputManager.CONTROLLER_ACTION.WALK_BACKWARDS))
            {
                action = InputManager.CONTROLLER_ACTION.WALK_BACKWARDS;
            }
            else if (InputManager.IsActionPressed(InputManager.CONTROLLER_ACTION.STRAFE_LEFT))
            {
                action = InputManager.CONTROLLER_ACTION.STRAFE_LEFT;
            }
            else if (InputManager.IsActionPressed(InputManager.CONTROLLER_ACTION.STRAFE_RIGHT))
            {
                action = InputManager.CONTROLLER_ACTION.STRAFE_RIGHT;
            }
            if (action != InputManager.CONTROLLER_ACTION.NONE)
            {
                FindMovementTarget(action);
            }
        }
        else
        {
            base.FixedUpdate();
        }
    }

    protected override void FindMovementTarget(GameManager.ACTION action)
    {
        FindMovementTarget(TransformGameManagerActionToControllerAction(action));
    }

    /// <summary>
    /// This method will check if the tile where the player is trying to move is available.
    /// </summary>
    private void FindMovementTarget(InputManager.CONTROLLER_ACTION movementDirection)
    {
        /// This method will rotate this object and the delegate. We need this code to ensure
        /// the camera does not get rotated when we rotate this object
        Quaternion currentRotation = cameraDelegate.transform.rotation;
        float yAngle = currentRotation.eulerAngles.y;
        float fixedYAngle = Mathf.Round(yAngle / 90) * 90f;
        transform.rotation = Quaternion.Euler(0, fixedYAngle, 0);
        cameraDelegate.transform.rotation = currentRotation;
        Vector3 targetDirection = TransformControllerActionToDirectionInWorldSpace(movementDirection);
        if (Pathfinding.CanMoveToPosition(transform.position, targetDirection, out hit))
        {
            target = transform.position + (targetDirection * GameManager.tileSize);
            isMoving = true;
        }
    }

    private Vector3 TransformControllerActionToDirectionInWorldSpace(InputManager.CONTROLLER_ACTION action)
    {
        Vector3 direction = Vector3.zero;
        switch (action)
        {
            case InputManager.CONTROLLER_ACTION.WALK_FORWARD:
                direction = transform.TransformDirection(Vector3.forward);
                break;
            case InputManager.CONTROLLER_ACTION.WALK_BACKWARDS:
                direction = transform.TransformDirection(Vector3.back);
                break;
            case InputManager.CONTROLLER_ACTION.STRAFE_LEFT:
                direction = transform.TransformDirection(Vector3.left);
                break;
            case InputManager.CONTROLLER_ACTION.STRAFE_RIGHT:
                direction = transform.TransformDirection(Vector3.right);
                break;
            default:
                break;
        }
        return direction;
    }

    private InputManager.CONTROLLER_ACTION TransformGameManagerActionToControllerAction(GameManager.ACTION action)
    {
        switch (action)
        {
            case GameManager.ACTION.NORTH:
                if (transform.forward == Vector3.forward)
                {
                    return InputManager.CONTROLLER_ACTION.WALK_FORWARD;
                }
                else if (transform.forward == Vector3.back)
                {
                    return InputManager.CONTROLLER_ACTION.WALK_BACKWARDS;
                }
                else if (transform.forward == Vector3.left)
                {
                    return InputManager.CONTROLLER_ACTION.STRAFE_RIGHT;
                }
                else if (transform.forward == Vector3.right)
                {
                    return InputManager.CONTROLLER_ACTION.STRAFE_LEFT;
                }
                break;
            case GameManager.ACTION.SOUTH:
                if (transform.forward == Vector3.forward)
                {
                    return InputManager.CONTROLLER_ACTION.WALK_BACKWARDS;
                }
                else if (transform.forward == Vector3.back)
                {
                    return InputManager.CONTROLLER_ACTION.WALK_FORWARD;
                }
                else if (transform.forward == Vector3.left)
                {
                    return InputManager.CONTROLLER_ACTION.STRAFE_LEFT;
                }
                else if (transform.forward == Vector3.right)
                {
                    return InputManager.CONTROLLER_ACTION.STRAFE_RIGHT;
                }
                break;
            case GameManager.ACTION.WEST:
                if (transform.forward == Vector3.forward)
                {
                    return InputManager.CONTROLLER_ACTION.STRAFE_LEFT;
                }
                else if (transform.forward == Vector3.back)
                {
                    return InputManager.CONTROLLER_ACTION.STRAFE_RIGHT;
                }
                else if (transform.forward == Vector3.left)
                {
                    return InputManager.CONTROLLER_ACTION.WALK_FORWARD;
                }
                else if (transform.forward == Vector3.right)
                {
                    return InputManager.CONTROLLER_ACTION.WALK_BACKWARDS;
                }
                break;
            case GameManager.ACTION.EAST:
                if (transform.forward == Vector3.forward)
                {
                    return InputManager.CONTROLLER_ACTION.STRAFE_RIGHT;
                }
                else if (transform.forward == Vector3.back)
                {
                    return InputManager.CONTROLLER_ACTION.STRAFE_LEFT;
                }
                else if (transform.forward == Vector3.left)
                {
                    return InputManager.CONTROLLER_ACTION.WALK_BACKWARDS;
                }
                else if (transform.forward == Vector3.right)
                {
                    return InputManager.CONTROLLER_ACTION.WALK_FORWARD;
                }
                break;
            default:
                break;
        }
        throw new UnityException("Player orientation was not snapped to the grid system");
    }

    public void SetBusyState(bool isBusy)
    {
        this.isBusy = isBusy;
        if (isBusy)
        {
            spinnerController.StartSpinning();
        }
        else
        {
            spinnerController.StopSpinning();
        }
    }
}
