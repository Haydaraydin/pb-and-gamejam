using UnityEngine;
using System.Collections;

public class LightFollow : MonoBehaviour {
	
	public Transform mainCharacter;
	
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		Vector3 direction = mainCharacter.position - transform.position;
		transform.rotation = Quaternion.LookRotation(direction);
	}
}
