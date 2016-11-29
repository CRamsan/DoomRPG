using UnityEngine;
using System.Collections;
using System;

public class DoorController : MonoBehaviour, IInteractable{

    public GameObject doorGameObject;
    public float speed = 5;

    private Vector3 closedPosition, openedPosition, targetPosition;
    private bool isMoving;
    private bool isClosed;

	// Use this for initialization
	void Start () {
        closedPosition = doorGameObject.transform.position;
        openedPosition = closedPosition + new Vector3(0, 4, 0);
        isMoving = false;
        isClosed = true;
    }
	
	// Update is called once per frame
	void Update () {
        if (isMoving)
        {
            float step = speed * Time.deltaTime;
            doorGameObject.transform.position = Vector3.MoveTowards(doorGameObject.transform.position, targetPosition, step);

            if (Vector3.Distance(doorGameObject.transform.position, targetPosition) == 0)
            {
                isMoving = false;
                if (doorGameObject.transform.position == openedPosition)
                {
                    gameObject.layer = LayerMask.NameToLayer(GameManager.LAYER_NON_BLOCKING);
                    isClosed = false;
                }
            }
        }
    }

    public void OpenDoor()
    {
        targetPosition = openedPosition;
        isMoving = true;
        isClosed = true;
        gameObject.layer = LayerMask.NameToLayer(GameManager.LAYER_BLOCKING);
    }

    public void CloseDoor()
    {
        targetPosition = closedPosition;
        isMoving = true;
        isClosed = true;
        gameObject.layer = LayerMask.NameToLayer(GameManager.LAYER_BLOCKING);
    }

    public bool IsOpened()
    {
        return !isClosed;
    }

    public bool IsMoving()
    {
        return isMoving;
    }

    public void Interact(GameObject caller)
    {
        if (isClosed)
            OpenDoor();
        else
            CloseDoor();
    }
}
