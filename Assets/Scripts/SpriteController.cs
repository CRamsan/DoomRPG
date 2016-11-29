using UnityEngine;
using System.Collections;

public class SpriteController : MonoBehaviour {

    private GameObject mainPlayer;

	// Use this for initialization
	void Start () {
        mainPlayer = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update () {
        transform.LookAt(new Vector3(mainPlayer.transform.position.x, transform.position.y, mainPlayer.transform.position.z));
	}
}
