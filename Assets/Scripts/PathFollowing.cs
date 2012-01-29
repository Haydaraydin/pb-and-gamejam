using UnityEngine;
using System.Collections;

public class PathFollowing : MonoBehaviour 
{

	public Vector3 initialPosition;
	public float defaultMoveSpeed = 1.0f;
	private float moveSpeed;
	public bool loop = false;
	public float maxDistance = 5.0f;
	
	public GameObject tunnelSpawner;
	
	// Use this for initialization
	void Start () 
	{
		transform.position = initialPosition;
		moveSpeed = defaultMoveSpeed;
	}
	
	// Update is called once per frame
	void Update () 
	{
		Vector3 newPos = transform.position;
		newPos += new Vector3(0, 0, moveSpeed*Time.deltaTime);
		
		if(loop && (newPos.z > maxDistance + initialPosition.z))
		{
			transform.position = initialPosition;
			
			tunnelSpawner.GetComponent<TunnelSpawnerScript>().Reset();
		}
		else
		{
			transform.position = newPos;
		}
		
	}
	
//	void OnGUI()
//	{
//		GUILayout.Label("Distance: " + transform.position.z);
//	}
	
	public void SetMoveSpeed(float newSpeed)
	{
		moveSpeed = newSpeed;
	}
	
	public float GetMoveSpeed()
	{
		return moveSpeed;
	}
}
