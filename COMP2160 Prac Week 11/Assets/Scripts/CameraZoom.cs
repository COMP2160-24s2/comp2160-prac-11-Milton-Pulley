using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraZoom : MonoBehaviour
{
	private float currCameraSize;
	[SerializeField] private float sensitivity;
	[SerializeField] private float minCameraSizePers = 50;
	[SerializeField] private float maxCameraSizePers = 90;
	[SerializeField] private float minCameraSizeOrth = 4;
	[SerializeField] private float maxCameraSizeOrth = 6;
	private Camera attachedCamera;

	private Actions cameraActions;
	private InputAction zoomAction;


	// Awake is called once the instance is created
	private void Awake()
	{
		cameraActions = new Actions();
		zoomAction = cameraActions.camera.zoom;

		attachedCamera = GetComponent<Camera>();
		if(attachedCamera == null)
		{
			Debug.LogError($"Error: This object has no \"{nameof(Camera)}\" component!");
		}

		if(attachedCamera.orthographic)
		{

		}
	}

	private void OnEnable()
	{
		zoomAction.Enable();
	}

	private void OnDisable()
	{
		zoomAction.Disable();
	}

	// Update is called once per frame
	private void Update()
	{
		float r = zoomAction.ReadValue<float>() * Time.deltaTime * sensitivity;
		Debug.Log(r);
		currCameraSize += r;
		currCameraSize = Mathf.Clamp01(currCameraSize);

		if(attachedCamera.orthographic)
		{
			attachedCamera.orthographicSize =
				Mathf.Lerp(minCameraSizeOrth, maxCameraSizeOrth, currCameraSize);
		}
		else
		{
			attachedCamera.fieldOfView =
				Mathf.Lerp(minCameraSizePers, maxCameraSizePers, currCameraSize);
		}
	}
}
