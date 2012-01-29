using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectSpawnerScript : MonoBehaviour {
	
	public Transform character;
	public int numSectors = 12;
	
	private float sectorSize;
	private int lastSector;
	private float currPosition;
	private int currLevel;
	private float timeInCurrLevel;
	
	[System.Serializable]
	public class ObjectType
	{
		public int typeID;
		public GameObject gameObject;
		public float objectLength = 0.1f;
		public float spawnRadius = 0.4f;
	}
	
	public ObjectType[] objects;
	
	[System.Serializable]
	public class PatternObject
	{
		public int objectTypeID;
		public float distanceToNext = 0.1f;
		public int numberOfObjects = 1;
		public bool useSectorOffset = false;
		public int sectorOffset = 0;
	}
	
	[System.Serializable]
	public class PatternData
	{
		public int typeID;
		public string description = "";
		public PatternObject[] objects;
	}
	
	public PatternData[] patterns;
	
	[System.Serializable]
	public class LevelPatternData
	{
		public int patternTypeID;
		public int probability;
	}
	
	[System.Serializable]
	public class LevelData
	{
		public string description = "";
		public float duration;
		public float minDistBetweenPatterns;
		public float maxDistBetweenPatterns;
		public LevelPatternData[] patterns;
		public float totalProbability;
	}
	
	public LevelData[] levels;
	
	private abstract class Action
	{
		public abstract float PerformAction();
	}
	
	private class WaitAction : Action
	{
		private float waitTime;
		
		public WaitAction(float time)
		{
			waitTime = time;
		}
		
		public override float PerformAction()
		{
			return waitTime;
		}
	}
	
	private class RandomWaitAction : Action
	{
		private float minWait, maxWait;
		
		public RandomWaitAction(float min, float max)
		{
			minWait = min;
			maxWait = max;
		}
		
		public override float PerformAction()
		{
			return Random.Range(minWait, maxWait);
		}	
	}
	
	private class SpawnAction : Action
	{
		private ObjectSpawnerScript ownerScript;
		private ObjectType objectType;
		private int numberOfObjectsToSpawn;
		private bool useObjectSectorOffset = false;
		private int objectSectorOffset = 0;
		
		public SpawnAction(ObjectSpawnerScript owner, ObjectType type, 
		                   int numberOfObjects, bool useSectorOffset, int sectorOffset)
		{
			ownerScript = owner;
			objectType = type;
			numberOfObjectsToSpawn = numberOfObjects;
			useObjectSectorOffset = useSectorOffset;
			objectSectorOffset = sectorOffset;
		}
		
		public override float PerformAction()
		{
			ownerScript.SpawnObjects(objectType.gameObject, objectType.spawnRadius,
			                        numberOfObjectsToSpawn, useObjectSectorOffset, objectSectorOffset);
			
			return objectType.objectLength;
		}
	}
	
	private float actionBuffer = 10.0f;
	Queue<Action> actionQueue;
	
	// Use this for initialization
	void Start ()
	{
		sectorSize = 2*Mathf.PI / numSectors;
		
		currLevel = 0;
		timeInCurrLevel = 0.0f;
		lastSector = 0;
		currPosition = character.position.z;
		
		VerifyData();
		
		actionQueue = new Queue<Action>();
		PerformActions(currPosition);
	}
	
	// Update is called once per frame
	void Update ()
	{	
		PerformActions(character.position.z);
	}
	
	void PerformActions(float yPos)
	{
		LevelData level = levels[currLevel];
		
		timeInCurrLevel += Time.deltaTime;
		
		if(timeInCurrLevel >= level.duration && currLevel < levels.Length-1)
		{
			timeInCurrLevel -= level.duration;
			++currLevel;
			level = levels[currLevel];
		}
		
		while(yPos > currPosition - actionBuffer)
		{
			if(actionQueue.Count == 0)
			{
				//Add Pattern
				float rand = Random.value;
				float prob = rand*level.totalProbability;
				
				foreach(LevelPatternData pattern in level.patterns)
				{
					if(prob < pattern.probability)
					{
						PatternData patData = GetPatternFromLevelPattern(pattern);
						
						foreach(PatternObject patObject in patData.objects)
						{
							ObjectType objType = GetObjectFromPatternObject(patObject);
							
							SpawnAction spawn = new SpawnAction(this, objType, patObject.numberOfObjects, patObject.useSectorOffset, patObject.sectorOffset);
							
							actionQueue.Enqueue(spawn);
							
							if(patObject.distanceToNext > 0)
								actionQueue.Enqueue(new WaitAction(patObject.distanceToNext));
						}
						
						break;
					}
					
					prob -= pattern.probability;
				}
				
				//Add Wait
				if(level.minDistBetweenPatterns < level.maxDistBetweenPatterns)
				{
					actionQueue.Enqueue(new RandomWaitAction(level.minDistBetweenPatterns, level.maxDistBetweenPatterns));
				}
			}
			
			Action next = actionQueue.Dequeue();
			
			currPosition += next.PerformAction();
		}
	}
	
	void SpawnObjects(GameObject obj, float spawnRadius, int numberOfObjects, bool useSectorOffset, int sectorOffset)
	{
		List<int> sectorsUsed = new List<int>();
		
		for(int i = 0; i < numberOfObjects; ++i)
		{
			int sector = 0;
			
			if(useSectorOffset && i == 0)
			{
				sector = lastSector + sectorOffset;
			}
			else
			{
				// Make sure we don't have more than one sector.
				bool sectorFound = false;
				
				while(!sectorFound)
				{
					sector = Random.Range(0, numSectors);
					
					bool sectorUsed = false;
					foreach(int usedSector in sectorsUsed)
					{
						if(usedSector == sector)
						{
							sectorUsed = true;
							break;
						}
					}
					
					if(!sectorUsed)
					{
						sectorFound = true;
					}
				}
			}
			
			sectorsUsed.Add(sector);
			
			float angle = sectorSize*sector;
			float x = Mathf.Sin(angle);
			float y = Mathf.Cos(angle);
			
			Vector3 spawnPosition = new Vector3(x*spawnRadius, y*spawnRadius, currPosition);
			
			Quaternion rotation = Quaternion.AngleAxis(180 - Mathf.Rad2Deg*angle, Vector3.forward);
			
			GameObject newObject;
			newObject = Instantiate(obj, spawnPosition, rotation) as GameObject;
			
			ObjectCleanUp cleanUp = newObject.GetComponent<ObjectCleanUp>();
			
			cleanUp.SetCharacter(character);
			
			lastSector = sector;
			
			// Save the initial angle for moving obstacles
			ObstacleRotation obsRot = newObject.GetComponent<ObstacleRotation>();
			if (obsRot != null)
			{
				obsRot.setCurrentAngle(angle);
				obsRot.setSpawnRadius(spawnRadius);
				
				int direction = Random.Range(0, 2);
				obsRot.setClockwiseRotation(direction == 0 ? true : false);
				if (direction == 1)
				{
					foreach (Transform child in newObject.transform)
					{
					    child.transform.RotateAroundLocal(Vector3.up, 180);
					}
				}
			}
		}
	}
	
	void VerifyData()
	{
		// Check Object Types for all Patterns
		foreach(PatternData pattern in patterns)
		{
			foreach(PatternObject patternObject in pattern.objects)
			{
				if(patternObject.numberOfObjects > numSectors-1)
				{
					throw new System.Exception("Too many objects being spawned in: " + patternObject.ToString());
				}
				
				bool objFound = false;
				foreach(ObjectType obj in objects)
				{
					if(patternObject.objectTypeID == obj.typeID)
					{
						objFound = true;
						break;
					}
				}
				
				if(!objFound)
				{
					throw new System.Exception("Error found in Object Spawner Pattern: " + pattern.ToString() + ":" + patternObject.ToString());
				}
			}
		}
		
		// Check Pattern Types for all Levels
		foreach(LevelData level in levels)
		{
			level.totalProbability = 0.0f;
			
			foreach(LevelPatternData levelPattern in level.patterns)
			{
				bool patFound = false;
				foreach(PatternData pattern in patterns)
				{
					if(levelPattern.patternTypeID == pattern.typeID)
					{
						patFound = true;
						break;
					}
				}
				
				if(!patFound)
				{
					throw new System.Exception("Error found in Object Spawner Level: " + level.ToString() + ":" + levelPattern.ToString());
				}
				
				level.totalProbability += levelPattern.probability;
			}
		}
	}
	
	private PatternData GetPatternFromLevelPattern(LevelPatternData levelPattern)
	{
		foreach(PatternData pattern in patterns)
		{
			if(pattern.typeID == levelPattern.patternTypeID)
				return pattern;
		}
		
		return null;
	}
	
	private ObjectType GetObjectFromPatternObject(PatternObject pattern)
	{
		foreach(ObjectType obj in objects)
		{
			if(obj.typeID == pattern.objectTypeID)
				return obj;
		}
		
		return null;
	}
	
	void OnGUI()
	{
		GUILayout.Label("Level: " + (currLevel + 1));
	}
}
