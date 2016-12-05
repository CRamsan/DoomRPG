using UnityEngine;
using System.Collections;

public class TileEnforcer : MonoBehaviour {

	// Use this for initialization
	void Start () {
        if (transform.position.x % 4 != 0 ||
            transform.position.z % 4 != 0)
        {
			Debug.Log("Bad position in " + transform.name + " at " + transform.position);
            throw new UnityException();
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
