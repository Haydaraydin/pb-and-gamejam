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
	}
	
	// Update is called once per frame
	void Update ()
	{
		float h = Input.GetAxisRaw("Horizontal");
		
		currAngle -= h * Time.deltaTime * rotationSpeed;
		
		if(currAngle > 2*Mathf.PI)
			currAngle -= 2*Mathf.PI;
		else if(currAngle < 0)
			currAngle += 2*Mathf.PI;
		
		CalcPosition();
	}
}
