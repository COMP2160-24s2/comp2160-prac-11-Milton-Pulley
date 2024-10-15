using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	[SerializeField] private Transform target;
	private float followerRadius = 1;

	// Start is called before the first frame update
	private void Start()
	{
		if(target == null)
		{
			Debug.LogError($"{nameof(Camera)} has no target to follow!");
		}
	}

	// LateUpdate is called after every Update has been called
	private void LateUpdate()
	{
		transform.position = target.position;
	}

	private void OnDrawGizmos()
	{
		if(!Application.isPlaying)
		{
			return;
		}
		
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(target.position, followerRadius);
	}
}
