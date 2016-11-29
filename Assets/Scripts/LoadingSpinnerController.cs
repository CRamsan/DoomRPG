using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class LoadingSpinnerController : MonoBehaviour {

    private SpriteRenderer spriteRenderer;
    private bool isSpinning;

	// Use this for initialization
	void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        isSpinning = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (isSpinning)
        {
            transform.Rotate(Vector3.back * Time.deltaTime * 500);
        }
    }

    public void StartSpinning()
    {
        spriteRenderer.transform.rotation = new Quaternion();
        spriteRenderer.enabled = true;
        isSpinning = true;
    }

    public void StopSpinning()
    {
        isSpinning = false;
        spriteRenderer.enabled = false;
    }
}
