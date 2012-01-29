using UnityEngine;
using System.Collections;

public class TunnelSpawnerScript : MonoBehaviour {
	
	public GameObject tunnelPrefab;
	public Transform character;
	
	public float segmentLength = 5.0f;
	public float pathBuffer = 10.0f;
	
	private float lastSpawnedSegment;
	
	// Use this for initialization
	void Start ()
	{
		Reset();
	}
	
	// Update is called once per frame
	void Update ()
	{
		PathFollowing pathFollow = character.GetComponent<PathFollowing>();
		if(pathFollow.GetPathPosition() > lastSpawnedSegment - pathBuffer)
		{			                               
			lastSpawnedSegment = SpawnTunnel(lastSpawnedSegment);
		}
	}
	
	float SpawnTunnel (float position)
	{	
		PathFollowing pathFollow = character.GetComponent<PathFollowing>();
		
		Vector3 point = new Vector3(0,0,0);
		Vector3 normal = new Vector3(0,0,0);
		pathFollow.GetPointAndNormalAt(position, ref point, ref normal);
		
		GameObject newTunnel;
		newTunnel = Instantiate(tunnelPrefab, point, Quaternion.LookRotation(normal)) as GameObject;
		TunnelRigHandler rigHandler = newTunnel.GetComponent<TunnelRigHandler>();
		return rigHandler.SetupTunnelRig(character, position);
	}
	
	public void Reset ()
	{	
		float currPos = SpawnTunnel(-5.0f);
		currPos = SpawnTunnel(currPos);
		lastSpawnedSegment = SpawnTunnel(currPos);
	}	
}
