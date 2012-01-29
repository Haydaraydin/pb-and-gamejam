using UnityEngine;
using System.Collections;

public class ObstacleRotation : MonoBehaviour 
{
	public float angularSpeed = 0.5f;
	
	private float currentAngle = 0.0f;
	private float spawnRadius = 0.0f;
	private bool clockwiseRotation = true;
	private float position;
	private Transform character;
	
	
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{			
		int directionModifier = clockwiseRotation ? 1 : -1;
		
		currentAngle += angularSpeed * Time.deltaTime * directionModifier;
			
		PathFollowing pathFollow = character.GetComponent<PathFollowing>();
		
		Vector3 point = new Vector3(0,0,0);
		Vector3 normal = new Vector3(0,0,0);
		pathFollow.GetPointAndNormalAt(position, ref point, ref normal);
		
		Vector3 yUp = new Vector3(0, 1, 0);
		Vector3 yUpProjected = yUp - Vector3.Project(yUp, normal);
		yUpProjected.Normalize();
		
		Quaternion posRotation = Quaternion.AngleAxis(Mathf.Rad2Deg*currentAngle, normal);
		
		Vector3 currPosition = posRotation * yUpProjected;
		currPosition *= spawnRadius;
		currPosition += point;
		
		Quaternion rotation = Quaternion.LookRotation(normal, point - currPosition);
			
		gameObject.transform.position = currPosition;

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
	
	public void SetPosition(float pos)
	{
		position = pos;
	}
	
	public void SetCharacter(Transform newCharacter)
	{
		character = newCharacter;
	}
}
