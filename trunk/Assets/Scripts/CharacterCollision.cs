using UnityEngine;
using System.Collections;

public class CharacterCollision : MonoBehaviour 
{
	private float timeLeftInSpeedPowerUp;
	private PathFollowing pathScript;
	
	private bool isAlive = true;
	private int numberOfCheese = 0;
	
	public AudioClip cheeseClip;

	// Use this for initialization
	void Start () 
	{
		pathScript = transform.parent.GetComponent<PathFollowing>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		// We died, pause the game and show the GUI
		if (Time.timeScale != 0.0f && !isAlive)
		{
			//Time.timeScale = 0.0f;
		}
		
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
		if (other.gameObject.name.StartsWith("StaticObstacle") || other.gameObject.name.Contains("Slime"))
		{
			audio.Play();
			isAlive = false;
		}
		else if (other.gameObject.name.StartsWith("SpeedPowerUp"))
		{
			SpeedPowerUpProperties properties = other.gameObject.GetComponent<SpeedPowerUpProperties>();
			CollectSpeedPowerup(properties.moveSpeedValue, properties.duration);
			Destroy(other.gameObject);
		}
		else if (other.gameObject.name.Contains("Cheese"))
		{
			audio.PlayOneShot(cheeseClip);
			numberOfCheese++;
			Destroy(other.gameObject);
		}
	}
	
	// Speed Power Up
	private void CollectSpeedPowerup(float speedValue, float duration)
	{
		pathScript.SetMoveSpeed(speedValue);
		timeLeftInSpeedPowerUp = duration;
	}
	
    public GUIStyle invisibleButton;
	
	// GUI
	void OnGUI()
	{
		GUILayout.Label("\n\n\nIs Alive: " + isAlive + " | Number of cheese: " + numberOfCheese);
		
		if (!isAlive && Time.timeScale == 0.0f)
		{
			GUILayout.Label("\n\n\n\n\n\n\n\n\nYOU HAVE DIED, BUT ATE A LOT OF CHEESE (" + numberOfCheese + 
			                " of them)\n TAP ANYWHERE TO RETURN TO THE MAIN MENU.");
	        if (GUI.Button(new Rect(0, 0, Screen.currentResolution.width, Screen.currentResolution.height), "", invisibleButton))
	        {
	            Application.LoadLevel(0);
	        }
		}
	}
}
