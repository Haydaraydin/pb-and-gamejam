using UnityEngine;
using System.Collections;

public class ObjectCleanUp : MonoBehaviour {
	
	public float cleanUpDistance = 10.0f;
	public Transform character;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Mathf.Abs(character.position.z - transform.position.z) > cleanUpDistance)
			Destroy(gameObject);
	}
	
	public void SetCharacter(Transform newCharacter)
	{
		character = newCharacter;
	}
}