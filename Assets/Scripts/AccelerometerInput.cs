using UnityEngine;
using System.Collections;

public class AccelerometerInput : MonoBehaviour 
{
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{		
		Vector3 aVec = Input.acceleration;
		
		//float deviceMag = Mathf.Sqrt(aVec.x*aVec.x+aVec.y*aVec.y+aVec.z*aVec.z);
		//float deviceAngle = Mathf.Round((180/Mathf.PI)*Mathf.Acos(aVec.y/deviceMag)) - 90;
		
		// Portrait mode, so axis are flipped
		float currentXf = aVec.x;  // store current X
		aVec.x = -aVec.y;   // X is now -Y
		aVec.y = currentXf;        // Y is original X
		
		Vector3 upVec = new Vector3(0, 1.0f, 0);
		float deviceAngle = Mathf.Acos(Vector3.Dot(aVec, upVec)/(aVec.magnitude*upVec.magnitude)) * Mathf.Rad2Deg;
		
		if (aVec.x < 0)
			deviceAngle = 180.0f + (180.0f - deviceAngle);
		
		// Draw the result in the text mesh
		TextMesh textMesh = (TextMesh)gameObject.GetComponent(typeof(TextMesh));
		
		textMesh.text = "Device angle: " + deviceAngle + " Vec: " + aVec;
	}
}
