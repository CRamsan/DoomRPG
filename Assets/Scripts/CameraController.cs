using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.VR;

/// <summary>
/// This controller provides functionality for the camera in both VR and non-VR enviroments. 
/// </summary>
public class CameraController : MonoBehaviour
{

    public float reticleMaxDistance = 4f;
    public GameObject reticlePrefab;

    private GameObject reticleOject;
	private GameObject lastSelectedElement;
	private bool isPrefabPresent;
	private bool isEventSystemPresent;
	private EventSystem eventSystem;
    
    // Use this for initialization
    void Start()
    {
		isPrefabPresent = reticlePrefab != null;
		if (isPrefabPresent)
        {
            reticleOject = Instantiate(reticlePrefab);
            reticleOject.transform.SetParent(transform);
        }

		eventSystem = EventSystem.current;
		isEventSystemPresent = eventSystem != null;
    }

    // Update is called once per frame
    void Update()
	{
		RaycastHit hit;
		if (isPrefabPresent) {
			bool reticleHit = RaycastFromGaze (out hit);
			float reticleDistance = reticleHit ? hit.distance : reticleMaxDistance;
			reticleOject.transform.position = transform.TransformPoint (new Vector3 (0, 0, reticleDistance));
		}

		// Use this to make the gaze also work as a pointer for UI elements
		if (isEventSystemPresent) {
			if (eventSystem.currentSelectedGameObject != null) {
				lastSelectedElement = EventSystem.current.currentSelectedGameObject;
			}

			if (InputManager.WasActionPressed (InputManager.CONTROLLER_ACTION.ACTION)) {
				if (eventSystem.currentSelectedGameObject == null) {
					eventSystem.SetSelectedGameObject (lastSelectedElement);
				}
			}
		}
	}

	public bool RaycastFromGaze(out RaycastHit hit, float distance) {
			Vector3 originPosition;

			// Check if origin position should be the camera when in non-VR and the center of the head when in VR
			if (VRSettings.enabled)
			{
				originPosition = transform.TransformPoint(InputTracking.GetLocalPosition(VRNode.CenterEye));
			}
			else
			{
				originPosition = transform.position;
			}

			// Raycast for the reticle should only be up to reticleMaxDistance
			Ray ray3D = new Ray(originPosition, transform.TransformDirection(Vector3.forward));
			return Physics.Raycast(ray3D, out hit, reticleMaxDistance);
	}

	public bool RaycastFromGaze(out RaycastHit hit) {
			return RaycastFromGaze(out hit, reticleMaxDistance);
	}
}