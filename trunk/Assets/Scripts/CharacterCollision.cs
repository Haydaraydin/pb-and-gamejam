using UnityEngine;
using System.Collections;
using System;

public class CharacterCollision : MonoBehaviour 
{
    public GUIStyle pauseButton;
	
	private float timeLeftInSpeedPowerUp;
	private PathFollowing pathScript;
	
	private bool isAlive = true;
	private int numberOfCheese = 0;
	
	public AudioClip cheeseClip;
	
	public Texture hudTexture;
	public Texture hudDeathTexture;
	public Texture ratIcon;
	public GUIStyle hudStyle;
	
	private int livesLeft = 3;
	
	private bool isPaused;


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
			Time.timeScale = 0.0f;
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
			transform.Find("Particle System").particleEmitter.Emit();
			audio.Play();
			livesLeft--;
			if (livesLeft <= 0)
				isAlive = false;
		}
		else if (other.gameObject.name.StartsWith("PowerUp"))
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
		GUI.DrawTexture(new Rect(0,0,1280,800), hudTexture);
	
		DateTime date = new DateTime((long)(Time.timeSinceLevelLoad * System.TimeSpan.TicksPerSecond));
				
		GUI.DrawTexture(new Rect(-70,67,106,31), ratIcon);
		GUI.Label(new Rect(30,67,300,100), " x " + livesLeft, hudStyle);
		GUI.Label(new Rect(177,67,300,100), String.Format("{0:HH:mm:ss}", date), hudStyle);
		GUI.Label(new Rect(197,95,300,100), "Cheese: " + numberOfCheese, hudStyle);
		

		if (!isAlive && Time.timeScale == 0.0f)
		{
			GUI.DrawTexture(new Rect(0,0,1280,800), hudDeathTexture);
			GUI.Label(new Rect(355,320,400,800), "You have died, rats!\nTap the screen to continue.", hudStyle);
			
	        if (GUI.Button(new Rect(0, 0, Screen.currentResolution.width, Screen.currentResolution.height), "", invisibleButton))
	        {
	            Application.LoadLevel(0);
	        }
		}
		
        else if (!isPaused && GUI.Button(new Rect(0, 0, 350, 125), "", invisibleButton))
        {
			Time.timeScale = 0.0f;
			isPaused = true;
        }
		else if(isPaused)
		{
			GUI.DrawTexture(new Rect(0,0,1280,800), hudDeathTexture);
			GUI.Label(new Rect(355,320,400,800), "Tap the screen to continue.", hudStyle);
			
	        if (GUI.Button(new Rect(0, 0, Screen.currentResolution.width, Screen.currentResolution.height), "", invisibleButton))
	        {
	            Time.timeScale = 1.0f;
				isPaused = false;
	        }
		}
	}
}
