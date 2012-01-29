using UnityEngine;
using System.Collections;

public class ObstacleRotation : MonoBehaviour 
{
	public float angularSpeed = 0.5f;
	
	private float currentAngle = 0.0f;
	private float spawnRadius = 0.0f;
	private bool clockwiseRotation = true;
	
	
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{			
		int directionModifier = clockwiseRotation ? 1 : -1;
		
		currentAngle += angularSpeed * Time.deltaTime * directionModifier;
		
		float x = Mathf.Sin(currentAngle);
		float y = Mathf.Cos(currentAngle);
			
		gameObject.transform.position = new Vector3(x*spawnRadius, y*spawnRadius, gameObject.transform.position.z);
			
		Quaternion rotation = Quaternion.AngleAxis(180 - Mathf.Rad2Deg*currentAngle, Vector3.forward);
		
		gameObject.transform.localRotation = rotation;
	}
	
	public void setCurrentAngle(float newAngle)
	{
		currentAngle = newAngle;
	}
	
	public void setSpawnRadius(float radius)
	{
		spawnRadius = radius;
	}
	
	public void setClockwiseRotation(bool val)
	{
		clockwiseRotation = val;
	}
}
