using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour 
{
	public Transform target;
	public float lagDistance = 0.5f;

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// LateUpdate used for camera motion so it happens after the character has moved.
	void LateUpdate () 
	{
		Vector3 newPos = target.position;
		newPos -= new Vector3(0, 0, lagDistance);
		
		transform.position = newPos;
	}
}
