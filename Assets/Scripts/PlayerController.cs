using UnityEngine;
using System.Collections;

public abstract class PlayerController : MonoBehaviour {

    public float speed = 20f;
    public int vision = 5;

    protected bool isMoving;
    protected Vector3 target;
    protected RaycastHit hit;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    protected virtual void FixedUpdate()
    {
        if (isMoving)
        {
            float step = 20f * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target, step);

            if (Vector3.Distance(transform.position, target) == 0)
            {
                isMoving = false;
                GameManager.Instance().ActionCompleted(this);
            }
        }
    }

    protected Vector3 TransformGameManagerActionToDirectionInWorldSpace(GameManager.ACTION action)
    {
        Vector3 direction = Vector3.zero;
        switch (action)
        {
            case GameManager.ACTION.NORTH:
                direction = Vector3.forward;
                break;
            case GameManager.ACTION.SOUTH:
                direction = Vector3.back;
                break;
            case GameManager.ACTION.WEST:
                direction = Vector3.left;
                break;
            case GameManager.ACTION.EAST:
                direction = Vector3.right;
                break;
            default:
                throw new UnityException("This method only takes actions that involve movement");
        }
        return direction;
    }

    protected GameManager.ACTION TransformDirectionToGameManagerAction(Vector2 direction)
    {
        if (direction.x == 1)
        {
            return GameManager.ACTION.EAST;
        }
        else if (direction.x == -1)
        {
            return GameManager.ACTION.WEST;
        }

        if (direction.y == 1)
        {
            return GameManager.ACTION.NORTH;
        }
        else if (direction.y == -1)
        {
            return GameManager.ACTION.SOUTH;
        }
        throw new UnityException("Direction not valid");
    }

    protected abstract void FindMovementTarget(GameManager.ACTION movementDirection);
}
