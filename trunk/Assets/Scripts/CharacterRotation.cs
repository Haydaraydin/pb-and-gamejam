using UnityEngine;
using System.Collections;

public class CharacterRotation : MonoBehaviour {
	
	public float offsetRadius;
	public float initialAngle;
	public float rotationSpeed = 2.0f;
	
	private float currAngle = 0.0f;
	
	private float angleVelocity = 0.0f;
	
	// Use this for initialization
	void Start ()
	{
		currAngle = Mathf.Deg2Rad * initialAngle;
		
		CalcPosition();
	}
	
	void CalcPosition()
	{	
		float x = Mathf.Sin(currAngle)*offsetRadius;
		float y = Mathf.Cos(currAngle)*offsetRadius;
		
		transform.localPosition = new Vector3(x, y, 0);
		
		transform.localRotation = Quaternion.AngleAxis(180 - Mathf.Rad2Deg*currAngle, Vector3.forward);
	}
	
	// Update is called once per frame
	void Update ()
	{	
		if (Application.platform == RuntimePlatform.Android) // Android Accelerometer support
		{			
			Vector3 aVec = Input.acceleration;
			
			// Portrait mode, so axis are flipped
			float currentXf = aVec.x;  	// store current X
			aVec.x = -aVec.y;   		// X is now -Y
			aVec.y = currentXf;        	// Y is original X
			
			// Only use x and y components to minimize the "flat surface case"
			Vector3 correctedVec = new Vector3(aVec.x, aVec.y, 0.0f);
			Vector3.Normalize(correctedVec);
			
			Vector3 upVec = new Vector3(0, 1.0f, 0);
			float newAngle = Mathf.Acos(Vector3.Dot(correctedVec, upVec)/(correctedVec.magnitude*upVec.magnitude));
			
			if (aVec.x < 0)
				newAngle = 2*Mathf.PI - newAngle;
			
			// Fallback
			if (float.IsNaN(currAngle))
				currAngle = 0.0f;
			if (float.IsNaN(newAngle))
				newAngle = 0.0f;
			
			// Angle dampening
			currAngle = Mathf.SmoothDampAngle(currAngle*Mathf.Rad2Deg, newAngle*Mathf.Rad2Deg, ref angleVelocity, 0.15f) * Mathf.Deg2Rad;
		}
		
		else // PC Mouse support	
		{
			float h = Input.GetAxisRaw("Horizontal");
			
			currAngle -= h * Time.deltaTime * rotationSpeed;
			
			if(currAngle > 2*Mathf.PI)
				currAngle -= 2*Mathf.PI;
			else if(currAngle < 0)
				currAngle += 2*Mathf.PI;
		}
		
		CalcPosition();
	}
}
