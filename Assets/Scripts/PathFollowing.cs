using UnityEngine;
using System.Collections;

public class PathFollowing : MonoBehaviour {

	public Vector3 initialPosition;
	public float moveSpeed = 1.0f;
	public float maxDistance = 5.0f;
	
	// Use this for initialization
	void Start () {
		transform.position = initialPosition;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 newPos = transform.position;
		newPos += new Vector3(0, 0, moveSpeed*Time.deltaTime);
		
		if(newPos.z > maxDistance + initialPosition.z)
			newPos = initialPosition;
		
		transform.position = newPos;
	}
}
