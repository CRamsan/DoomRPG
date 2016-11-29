using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding {
    private GameObject placeholderPrefab;

    public class Node {

		public int x, y, depth;
		public GameObject occupant;

		public void SetOccupant(GameObject occupant) {
			this.occupant = occupant;
			this.x = (int)this.occupant.transform.position.x;
			this.y = (int)this.occupant.transform.position.z;
            NPCController controller = occupant.GetComponent<NPCController>();
            if (controller != null)
            {
                controller.SetNode(this);
            }
		}
	}

	public class PathGenerator
	{
        private HashSet<Node> nodesSet;
        private GameObject placeholderPrefab;

        public PathGenerator()
		{
            nodesSet = new HashSet<Node>();
            placeholderPrefab = Resources.Load("NodePlaceholder", typeof(GameObject)) as GameObject;
        }

        public HashSet<Node> GetMap()
        {
            return nodesSet;
        }

        public List<GameObject> FindTargets(PlayerController player) {

            HashSet<Node> traversedNodesSet = new HashSet<Node>();
            Queue<Node> toTraverseQueue = new Queue<Node>();
            List<GameObject> placeholderList = new List<GameObject>();
            List<GameObject> possibleTargets = new List<GameObject>();

            int range = player.vision;
            Node initialNode = this.CreateNode(     player.gameObject.transform.position.x, 
                                                    player.gameObject.transform.position.z, 
                                                    player.gameObject);
            initialNode.depth = 0;
            placeholderList.Add(initialNode.occupant);
            toTraverseQueue.Enqueue(initialNode);
            Vector3[] directions = new Vector3[4] { Vector3.forward, Vector3.left, Vector3.right, Vector3.back };

            while (toTraverseQueue.Count > 0) {
				Node currentNode = toTraverseQueue.Dequeue();
				Vector3 nodeOrigin = new Vector3 (currentNode.x, GameManager.verticalOffset, currentNode.y);
				foreach (Vector3 direction in directions) {
					RaycastHit hit;
                    bool isTraversable = true;
                    GameObject occupant = null;
                    if (!CanMoveToPosition(nodeOrigin, direction, out hit))
                    {
                        GameObject collider = hit.collider.gameObject;
                        if (collider.GetComponent<PlayerController>() != null)
                        {
                            PlayerController playerFound = collider.GetComponent<PlayerController>();
                            occupant = playerFound.gameObject;
                        }
                        else
                        {
                            isTraversable = false;
                        }
                    }
                    if (isTraversable)
                    {
                        Node newNode = CreateNode(nodeOrigin.x + direction.x * GameManager.tileSize,
                                nodeOrigin.z + direction.z * GameManager.tileSize,
                                occupant);
                        newNode.depth = currentNode.depth + 1;
                        if (newNode.depth < range)
                            toTraverseQueue.Enqueue(newNode);
                    }
                }
                traversedNodesSet.Add(currentNode);
            }
            foreach (Node node in traversedNodesSet)
            {
                GameObject tempOccupant = node.occupant;
                if (tempOccupant.name.Contains(placeholderPrefab.name)) {
                    GameObject.Destroy(tempOccupant);
                }
            }
            return possibleTargets;
        }

        private Node CreateNode(float x, float z, GameObject occupant)
        {
            Node initialNode = new Node();
            GameObject placeHolder;
            if (occupant == null)
            {
                placeHolder = GameObject.Instantiate(placeholderPrefab) as GameObject;
            }
            else
            {
                placeHolder = occupant;
            }
            placeHolder.transform.position = new Vector3(x, GameManager.verticalOffset, z);
            initialNode.SetOccupant(placeHolder);

            return initialNode;
        }

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
}
