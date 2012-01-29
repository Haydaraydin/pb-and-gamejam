using UnityEngine;
using System.Collections;

public class TriggerEntryAnim : MonoBehaviour 
{
	public GameObject objectToPlayOn;
	public string entryClip;
	public float distanceFromCamera = 5.0f;
	
	private Camera mainCam;
	private bool played;
	
	// Use this for initialization
	void Start () 
	{
		mainCam = (Camera)FindObjectOfType(typeof(Camera));
		
		played = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		Vector3 camPos = mainCam.transform.position;
		
		if(Vector3.Distance(camPos, transform.position) < distanceFromCamera && !played 
		   && objectToPlayOn != null && objectToPlayOn.animation != null)
		{
			objectToPlayOn.animation.Play(entryClip);
			
			if (objectToPlayOn.audio != null)
				objectToPlayOn.audio.Play();
			
			played = true;
		}
	}
}
