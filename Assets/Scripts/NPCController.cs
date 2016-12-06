using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[RequireComponent (typeof (UtilFlashOnHit))]
public class NPCController : PlayerController, IInteractable {

    public GameObject canvasGameObject;

    private Vector3 enemyLocation;
	private bool npcHasText;
	private UtilFlashOnHit flashOnHit; 

    // Use this for initialization
    void Start () {
        GameManager.Instance().RegisterNPC(this);
		npcHasText = canvasGameObject != canvasGameObject;
		flashOnHit = GetComponent<UtilFlashOnHit> ();
    } 
		
    public void FindNextAction()
    {
        GameManager.ACTION action = GameManager.ACTION.PASS;

        List<GameObject> playersNearby = GameManager.Instance().FindNearPlayers(this) ;

		GameObject target = null;
		bool enemyIsVisible = false;
		if (playersNearby.Count > 0) {
			target = playersNearby [0];
			enemyIsVisible = Pathfinding.IsObjectVisible(transform.position, target, 25);
		}
        if (enemyIsVisible || enemyLocation.magnitude != 0)
        {
            if (enemyIsVisible)
            {
				enemyLocation = target.transform.position;
            }
            Vector3 targetDirection = enemyLocation - transform.position;
            int vX, vY;
            if (Math.Abs(targetDirection.x) > Math.Abs(targetDirection.z))
            {
                vX = targetDirection.x > 0 ? 1 : -1;
                vY = 0;
            }
            else
            {
                vX = 0;
                vY = targetDirection.z > 0 ? 1 : -1;
            }
            Vector2 targetDirection2D = new Vector2(vX, vY);
            action = TransformDirectionToGameManagerAction(targetDirection2D);
            if (!enemyIsVisible)
            {
                enemyLocation = Vector3.zero;
            }
        }
        else
        {
            int result = UnityEngine.Random.Range(0, 4);
            switch (result)
            {
                case 0:
                    action = GameManager.ACTION.NORTH;
                    break;
                case 1:
                    action = GameManager.ACTION.SOUTH;
                    break;
                case 2:
                    action = GameManager.ACTION.EAST;
                    break;
                case 3:
                    action = GameManager.ACTION.WEST;
                    break;
                default:
                    throw new UnityException("Random result is invalid");
            }
        }
        FindMovementTarget(action);
    }

    public void Interact(GameObject caller)
    {
		if (npcHasText) {
			canvasGameObject.SetActive (!canvasGameObject.activeSelf);
		} else {
			flashOnHit.StartFlash ();
		}
    }
		
    /// <summary>
    /// Provide a Vector3 direction and this method will set the target for this movement.
    /// </summary>
    protected override void FindMovementTarget(GameManager.ACTION action)
    {
        if (action != GameManager.ACTION.NORTH &&
            action != GameManager.ACTION.SOUTH &&
            action != GameManager.ACTION.WEST &&
            action != GameManager.ACTION.EAST)
        {
            throw new UnityException("This method only takes actions that involve movement");
        }

        Vector3 targetDirection = TransformGameManagerActionToDirectionInWorldSpace(action);
        if (Pathfinding.CanMoveToPosition(transform.position, targetDirection, out hit))
        {
            target = transform.position + (targetDirection * GameManager.tileSize);
        }
        else
        {
            target = transform.position;
        }
        isMoving = true;
    }
}
