using UnityEngine;
using System.Collections;

public class ObjectCleanUp : MonoBehaviour {
	
	public float cleanUpDistance = 10.0f;
	public Transform character;
	public float position;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		PathFollowing pathFollow = character.GetComponent<PathFollowing>();
		
		if(pathFollow.GetPathPosition() > position + cleanUpDistance)
			Destroy(gameObject);
	}
	
	public void SetupClean(Transform newCharacter, float newPosition)
	{
		character = newCharacter;
		position = newPosition;
	}
}
