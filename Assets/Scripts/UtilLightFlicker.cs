using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Light))]
public class UtilLightFlicker : MonoBehaviour {

    private Light targetLight;
    private float fullIntensity;

	// Use this for initialization
	void Start () {
        targetLight = GetComponent<Light>();
        fullIntensity = targetLight.intensity;
	}
	
	// Update is called once per frame
	void Update () {
        // This is really unoptimized, we could use coroutines rather than running this code on evey frame.
        if (Random.value > 0.95)
        {
            if (targetLight.intensity == fullIntensity)
            {
                targetLight.intensity = 0f;
            }
            else
            {
                targetLight.intensity = fullIntensity;
            }
        }
    }
}
