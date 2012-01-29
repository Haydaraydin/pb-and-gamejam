using UnityEngine;
using System.Collections;

public class CharacterCollision : MonoBehaviour 
{
	private float timeLeftInSpeedPowerUp;
	private PathFollowing pathScript;
	
	private bool isAlive = true;

	// Use this for initialization
	void Start () 
	{
		pathScript = transform.parent.GetComponent<PathFollowing>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (timeLeftInSpeedPowerUp > 0.0f)
		{
			timeLeftInSpeedPowerUp -= Time.deltaTime;
			
			if (timeLeftInSpeedPowerUp	<= 0.0f)
			{
				pathScript.SetMoveSpeed(pathScript.defaultMoveSpeed);
			}
		}
	}
	
	// Fixed update
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.name.StartsWith("StaticObstacle"))
		{
			isAlive = false;
		}
		else if (other.gameObject.name.StartsWith("SpeedPowerUp"))
		{
			SpeedPowerUpProperties properties = other.gameObject.GetComponent<SpeedPowerUpProperties>();
			CollectSpeedPowerup(properties.moveSpeedValue, properties.duration);
			properties.DestroyOnNextUpdate();
		}
	}
	
	// Speed Power Up
	private void CollectSpeedPowerup(float speedValue, float duration)
	{
		pathScript.SetMoveSpeed(speedValue);
		timeLeftInSpeedPowerUp = duration;
	}
	
	
	// GUI
	void OnGUI()
	{
		GUILayout.Label("\n\n\nIs Alive: " + isAlive);
	}
}
