using UnityEngine;
using System.Collections;

public class TunnelRigHandler : MonoBehaviour {
	
	public float cleanUpDistance = 10.0f;
	
	private Transform character;
	private float position;
	private PathFollowing.Segment segment;
	
	// Use this for initialization
	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update ()
	{
		PathFollowing pathFollow = character.GetComponent<PathFollowing>();
		if(pathFollow.GetPathPosition() > position + cleanUpDistance)
		{
			if(segment != null)
				pathFollow.RemoveSegment(segment);
			
			Destroy(gameObject);
		}
	}
	
	public float SetupTunnelRig(Transform newCharacter, float pos)
	{
		character = newCharacter;
		position = pos;
		
		float currPosition = position;
		
		Transform bone1 = transform.Find("main_tunnel/Tunnel_Bone_01");
		Transform bone2 = transform.Find("main_tunnel/Tunnel_Bone_01/Tunnel_Bone_02");
		Transform bone3 = transform.Find("main_tunnel/Tunnel_Bone_01/Tunnel_Bone_02/Tunnel_Bone_03");
		Transform boneEnd = transform.Find("main_tunnel/Tunnel_Bone_01/Tunnel_Bone_02/Tunnel_Bone_03/Tunnel_Bone_End");
		
		//Create a Segment from the Tunnel
		segment = new PathFollowing.Segment();
		
		segment.subSegments = new PathFollowing.SubSegment[3];
		segment.startDistance = currPosition;
		
		// SubSegment 1
		PathFollowing.SubSegment subSegment = new PathFollowing.SubSegment();
		subSegment.startDistance = currPosition;
		subSegment.startPoint = bone1.position;
		subSegment.endPoint = bone2.position;
		subSegment.endDistance = currPosition + Vector3.Distance(subSegment.startPoint, subSegment.endPoint);
		segment.subSegments[0] = subSegment;
		currPosition = subSegment.endDistance;
		
		// SubSegment 2
		subSegment = new PathFollowing.SubSegment();
		subSegment.startDistance = currPosition;
		subSegment.startPoint = bone2.position;
		subSegment.endPoint = bone3.position;
		subSegment.endDistance = currPosition + Vector3.Distance(subSegment.startPoint, subSegment.endPoint);
		segment.subSegments[1] = subSegment;
		currPosition = subSegment.endDistance;
		
		// SubSegment 3
		subSegment = new PathFollowing.SubSegment();
		subSegment.startDistance = currPosition;
		subSegment.startPoint = bone3.position;
		subSegment.endPoint = boneEnd.position;
		subSegment.endDistance = currPosition + Vector3.Distance(subSegment.startPoint, subSegment.endPoint);
		segment.subSegments[2] = subSegment;
		
		segment.endDistance = subSegment.endDistance;
		
		PathFollowing pathFollow = character.GetComponent<PathFollowing>();
		pathFollow.AddSegment(segment);
		
		return segment.endDistance;
	}
}
