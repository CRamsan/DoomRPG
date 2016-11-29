using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    private static GameManager instance;

	public static int tileSize = 4;
	public static int verticalOffset = 2;

	public GameObject player;

    public GameObject NPCList;
    public GameObject InteractableList;

    public List<NPCController> allPlayers;
    public Queue<NPCController> npcQueue;

    private PlayerController currentPlayer;
    public PlayerController mainPlayer;
    private Pathfinding.PathGenerator pathFinding;

    public enum ACTION
    {
        NORTH,
        SOUTH,
        EAST,
        WEST,
        ATTACK,
        ACTION,
        PASS
    }

    public static string LAYER_NON_BLOCKING = "NonBlockingObject";
    public static string LAYER_BLOCKING = "BlockingObject";

    public static GameManager Instance()
    {
        return instance;
    }

	// Use this for initialization
	void Start () {
        instance = this;

        pathFinding = new Pathfinding.PathGenerator();
        allPlayers = new List<NPCController>();
        npcQueue = new Queue<NPCController>();
        mainPlayer = player.GetComponent<MainPlayerController>();
        currentPlayer = mainPlayer;
    }
	
    public List<GameObject> FindNearPlayers(PlayerController player)
    {
        return pathFinding.FindTargets(player);
    }

    public void RegisterNPC(NPCController controller)
    {
        if (!allPlayers.Contains(controller))
        {
            allPlayers.Add(controller);
        }
        else
        {
            throw new UnityException("NPC already was registered");
        }
    }

    public void ActionCompleted(PlayerController player) {
        if (currentPlayer != player)
            throw new UnityException();

        if (player.GetType().Equals(typeof(MainPlayerController))) {
            ((MainPlayerController)player).SetBusyState(true);
            allPlayers.Sort(delegate (NPCController player1, NPCController player2)
            {
                return player1.speed.CompareTo(player2.speed);
            });
            foreach (NPCController npcPlayer in allPlayers)
            {
                npcQueue.Enqueue(npcPlayer);
            }
        }
        else if (player.GetType().Equals(typeof(NPCController)))
        {
            npcQueue.Dequeue();
        }
        if (npcQueue.Count > 0)
        {
            NPCController nextNPCPlayer = npcQueue.Peek();
            nextNPCPlayer.FindNextAction();
            currentPlayer = nextNPCPlayer;
        }
        else
        {
            currentPlayer = mainPlayer;
            if (currentPlayer.GetType().Equals(typeof(MainPlayerController)))
            {
                ((MainPlayerController)currentPlayer).SetBusyState(false);
            }
            else
            {
                throw new UnityException("Main player needs to be set to an instance of MainPlayerController");
            }
        }
    }
}
