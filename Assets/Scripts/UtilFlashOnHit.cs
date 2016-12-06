using UnityEngine;
using System.Collections;

public class UtilFlashOnHit : MonoBehaviour {

	public Color hitColor;

	private Color startingColor;
	private Material meshMaterial;

	void Start() {
		MeshRenderer renderer = GetComponent<MeshRenderer> ();
		if (renderer) {
			throw new UnityException ();
		}
		meshMaterial = renderer.material;
		startingColor = meshMaterial.color;
	}

	IEnumerator PrivateStartFlash() 
	{
		
		for (int i = 0; i < 5; i++)
		{
			meshMaterial.color = hitColor;
			yield return new WaitForSeconds(.1f);
			meshMaterial.color = startingColor; 
			yield return new WaitForSeconds(.1f);
		}
	}

	public void StartFlash(){
		StartCoroutine (PrivateStartFlash());
	}
}
