/**
 * A singleton class to allow point-and-click movement of the marble.
 * 
 * It publishes a TargetSelected event which is invoked whenever a new target is selected.
 * 
 * Author: Malcolm Ryan
 * Version: 1.0
 * For Unity Version: 2022.3
 */

using UnityEngine;
using UnityEngine.InputSystem;

// note this has to run earlier than other classes which subscribe to the TargetSelected event
[DefaultExecutionOrder(-100)]
public class UIManager : MonoBehaviour
{
	private float crosshairHoverDistance = 0.05f;
	[SerializeField] private LayerMask floorLayer;
	private Camera mainCam;
	private Vector2 mousePos;
	[SerializeField] private bool moveCrosshairByDelta;
	
#region UI Elements
	[SerializeField] private Transform crosshair;
	[SerializeField] private Transform target;
#endregion 

#region Singleton
	static private UIManager instance;
	static public UIManager Instance
	{
		get { return instance; }
	}
#endregion 

#region Actions
	private Actions actions;
	private InputAction mouseAction;
	private InputAction deltaAction;
	private InputAction selectAction;
#endregion

#region Events
	public delegate void TargetSelectedEventHandler(Vector3 worldPosition);
	public event TargetSelectedEventHandler TargetSelected;
#endregion

#region Init & Destroy
	void Awake()
	{
		if (instance != null)
		{
			Debug.LogError("There is more than one UIManager in the scene.");
		}

		instance = this;

		actions = new Actions();
		mouseAction = actions.mouse.position;
		deltaAction = actions.mouse.delta;
		selectAction = actions.mouse.select;

		Cursor.visible = false;
		target.gameObject.SetActive(false);

		mainCam = Camera.main;
		if(mainCam == null)
		{
			Debug.LogError("Error: could not find Camera.main!");
		}
	}

	void OnEnable()
	{
		actions.mouse.Enable();
	}

	void OnDisable()
	{
		actions.mouse.Disable();
	}
#endregion Init

#region Update
	void Update()
	{
		MoveCrosshair();
		SelectTarget();
	}

	private void MoveCrosshair() 
	{
		//Vector2 v = mousePos;
		if(moveCrosshairByDelta)
		{
			mousePos += deltaAction.ReadValue<Vector2>();
			//Vector2 v2 = deltaAction.ReadValue<Vector2>();
			//Debug.Log(v2);
			// constant different depending on screen resolution AND editor scale
			//mousePos += v2;// * (Screen.width * 0.5f);
		}
		else
		{
			mousePos = mouseAction.ReadValue<Vector2>();
		}
		if(Input.GetKeyDown(KeyCode.P))
		{
			mousePos = mouseAction.ReadValue<Vector2>();
			//Debug.Log($"{Screen.width}, {Screen.height}");
		}
		//Debug.Log($"{v}, {mousePos}, {(mousePos -  v)}, {deltaAction.ReadValue<Vector2>()}");

		Ray ray = mainCam.ScreenPointToRay(mousePos);
		RaycastHit hit;
		if(Physics.Raycast(ray, out hit, floorLayer))
		{
			crosshair.position = hit.point + (hit.normal * crosshairHoverDistance);
		}
	}

	// breaks mouse delta
	private void OnApplicationFocus(bool focused)
	{
		//Application.Onn += (focused) => 
		//{
			if(focused && moveCrosshairByDelta)
			{
				mousePos = mouseAction.ReadValue<Vector2>();
				Debug.Log($"{Screen.width}, {Screen.height}");
			}
		//};
	}

	private void SelectTarget()
	{
		if (selectAction.WasPerformedThisFrame())
		{
			// set the target position and invoke 
			target.gameObject.SetActive(true);
			target.position = crosshair.position;     
			TargetSelected?.Invoke(target.position);       
		}
	}

#endregion Update

}
