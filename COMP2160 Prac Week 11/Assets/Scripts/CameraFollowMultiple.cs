using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowMultiple : MonoBehaviour
{
	[SerializeField] private List<Transform> targets = new List<Transform>();
	private Vector3 averagePosition;
	private float followerRadius = 1;

	// Start is called before the first frame update
	private void Start()
	{
		if(targets == null)
		{
			targets = new List<Transform>();
			Debug.LogWarning(
				$"Warning: {nameof(CameraFollowMultiple)} had uninitialised targets list!");
		}
		if(targets.Count < 2)
		{
			Debug.LogError(
				$"Error: {nameof(CameraFollowMultiple)} has no target to follow!");
		}
	}

	// LateUpdate is called after every Update has been called
	private void LateUpdate()
	{
		averagePosition = Vector3.zero;
		foreach(Transform t in targets)
		{
			averagePosition += t.position;
		}
		averagePosition /= targets.Count;

		transform.position = averagePosition;
	}

	private void OnDrawGizmos()
	{
		if(!Application.isPlaying)
		{
			return;
		}

		foreach(Transform t in targets)
		{
			Gizmos.color = Color.red;
			Gizmos.DrawLine(t.position, averagePosition);

			Gizmos.color = Color.green;
			Gizmos.DrawWireSphere(t.position, followerRadius);
		}

		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(averagePosition, followerRadius);
	}
}
