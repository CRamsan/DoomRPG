using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding {

	public static List<GameObject> FindNearTargets(PlayerController player) {

		List<GameObject> possibleTargets = new List<GameObject>();

		Collider[] hitColliders = Physics.OverlapSphere(player.transform.position, player.visionRange * GameManager.tileSize);
		foreach (Collider collider in hitColliders) {
			GameObject foundGameObject = collider.gameObject;
			if (foundGameObject.GetComponent<PlayerController> () != null) {
				if (IsObjectVisible (player.transform.position, foundGameObject, player.visionRange * GameManager.tileSize)) {
					possibleTargets.Add (foundGameObject);
				}
			}
		}
		return possibleTargets;
	}

    public static bool CanMoveToPosition(Vector3 origin, Vector3 direction, out RaycastHit hit)
    {
        Ray ray3D = new Ray(origin, direction);
        bool reticleHit = Physics.Raycast(ray3D, out hit, GameManager.tileSize);
        if (reticleHit)
        {
            DoorController door = hit.collider.gameObject.GetComponent<DoorController>();
            if (door == null || !door.IsOpened())
            {
                return false;
            }
        }
        return true;
    }

    public static bool IsObjectVisible(Vector3 origin, GameObject target, float range)
    {
        if (Vector3.Distance(origin, target.transform.position) < range)
        {
            RaycastHit hit;
            if (Physics.Raycast(origin, (target.transform.position - origin), out hit, range, LayerMask.NameToLayer(GameManager.LAYER_NON_BLOCKING)))
            {
                if (hit.transform == target.transform)
                {
                    return true;
                }
            }
        }
        return false;
    }

	public static GameObject GetVisibleGameObject(Vector3 origin, Vector3 target, float range)
	{
		if (Vector3.Distance(origin, target) < range)
		{
			RaycastHit hit;
			if (Physics.Raycast(origin, (target - origin), out hit, range, LayerMask.NameToLayer(GameManager.LAYER_NON_BLOCKING)))
			{
				GameObject foundObject = hit.transform.gameObject;
				if (foundObject.GetComponent<PlayerController>() != null)
				{
					return foundObject;
				}
			}
		}
		return null;
	}
}
