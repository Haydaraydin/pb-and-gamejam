using UnityEngine;
using System.Collections;

public class CharacterRotation : MonoBehaviour {
	
	public float offsetRadius;
	public float initialAngle;
	public float rotationSpeed = 2.0f;
	
	private float currAngle;
	
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
			
			Vector3 upVec = new Vector3(0, 1.0f, 0);
			currAngle = Mathf.Acos(Vector3.Dot(aVec, upVec)/(aVec.magnitude*upVec.magnitude));
			
			if (aVec.x < 0)
				currAngle = 2*Mathf.PI - currAngle;
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
	
	void OnGUI()
	{
		GUILayout.Label("\nCurrent angle: " + currAngle);
	}
}
