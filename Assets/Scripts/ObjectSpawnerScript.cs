using UnityEngine;
using System.Collections;

public class ObjectSpawnerScript : MonoBehaviour {
	
	public Transform character;
	public float spawnRadius;
	public GameObject staticObstacle;
	public float obstacleLength;
	
	public int numSectors = 12;
	public int minObstacles = 1;
	public int maxObstacles = 3;
	private float sectorSize;
	
	// Use this for initialization
	void Start ()
	{
		sectorSize = 2*Mathf.PI / numSectors;
	}
	
	// Update is called once per frame
	void Update ()
	{}
	
	public void SpawnObjects(Vector3 position, float segmentLength)
	{
		int numObstacles = Random.Range(minObstacles, maxObstacles+1);
		
		float subSegmentLength = segmentLength/numObstacles;
		float subSegmentStart = position.z;
				
		for(int i = 0; i < numObstacles; ++i)
		{
			float zPosition = Random.Range(subSegmentStart, subSegmentStart + subSegmentLength - obstacleLength);
			int sector = Random.Range(0, numSectors);
			
			SpawnObstacle(zPosition, sector);
			subSegmentStart += subSegmentLength;
		}
	}
	
	void SpawnObstacle(float zPosition, int sector)
	{
		float angle = sectorSize*sector;
		float x = Mathf.Sin(angle);
		float y = Mathf.Cos(angle);
		
		Vector3 spawnPosition = new Vector3(x*spawnRadius, y*spawnRadius, zPosition);
		
		Quaternion rotation = Quaternion.AngleAxis(180 - Mathf.Rad2Deg*angle, Vector3.forward);
		
		GameObject newObstacle;
		newObstacle = Instantiate(staticObstacle, spawnPosition, rotation) as GameObject;
		
		newObstacle.GetComponent<ObjectCleanUp>().SetCharacter(character);
	}
}
