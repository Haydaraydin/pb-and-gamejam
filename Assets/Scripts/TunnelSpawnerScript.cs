using UnityEngine;
using System.Collections;

public class TunnelSpawnerScript : MonoBehaviour {
	
	public GameObject tunnelPrefab;
	public Transform character;
	
	public float segmentLength = 5.0f;
	
	private float lastSpawnedSegment;
	
	// Use this for initialization
	void Start ()
	{
		Reset();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(character.position.z > lastSpawnedSegment)
		{
			Vector3 spawnPos = new Vector3(
								character.position.x,
		                    	character.position.y,
		                    	lastSpawnedSegment + segmentLength);
			                               
			SpawnTunnel(spawnPos);
			
			lastSpawnedSegment = spawnPos.z;
		}
	}
	
	void SpawnTunnel (Vector3 position)
	{	
		GameObject newTunnel;
		newTunnel = Instantiate(tunnelPrefab, position, Quaternion.identity) as GameObject;
		ObjectCleanUp cleanUp = newTunnel.GetComponent<ObjectCleanUp>();
		cleanUp.SetCharacter(character);
	}
	
	public void Reset ()
	{
		Vector3 posBack = new Vector3(
								character.position.x,
		                    	character.position.y,
		                    	character.position.z - segmentLength);
		
		Vector3 posMiddle = character.position;
		
		Vector3 posFront = new Vector3(
								character.position.x,
		                    	character.position.y,
		                    	character.position.z + segmentLength);
		
		SpawnTunnel(posBack);
		SpawnTunnel(posMiddle);
		SpawnTunnel(posFront);
		
		lastSpawnedSegment = posFront.z;
	}	
}
