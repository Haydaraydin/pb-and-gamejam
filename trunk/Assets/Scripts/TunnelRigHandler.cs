using UnityEngine;
using System.Collections;

public class TunnelRigHandler : MonoBehaviour {
	
	public float cleanUpDistance = 10.0f;
	
	public float noTurnPercentChance = 70.0f;
	
	public float harshTurnProbability = 30.0f;
	public float sTurnProbability = 30.0f;
	public float softTurnProbability = 40.0f;
	
	private float totalTurnProbability;
	
	private Transform character;
	private float position;
	private PathFollowing.Segment segment;
	
	// Use this for initialization
	void Start ()
	{
		totalTurnProbability = harshTurnProbability + sTurnProbability + softTurnProbability;
	}
	
	// Update is called once per frame
	void Update ()
	{
		PathFollowing pathFollow = character.GetComponent<PathFollowing>();
		if(pathFollow != null && pathFollow.GetPathPosition() > position + cleanUpDistance)
		{
			if(segment != null)
				pathFollow.RemoveSegment(segment);
			
			Destroy(gameObject);
		}
	}
	
	private Vector3 GetArc(Vector3 normal, float vertAngle, float normAngle)
	{
		Vector3 yUp = new Vector3(0, 1, 0);
		Vector3 yUpProjected = yUp - Vector3.Project(yUp, normal);
		
		Vector3 newVec = Mathf.Cos(vertAngle)*normal + Mathf.Sin(vertAngle)*yUpProjected;
		newVec.Normalize();
		
		Quaternion newVecRot = Quaternion.AngleAxis(normAngle, normal);
		
		newVec = newVecRot*newVec;
		
		return newVec;
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
		
		//Handle Turns
		float noTurnChance = Random.value;
		if(noTurnChance * 100.0f > noTurnPercentChance)
		{
			Vector3 normal = bone2.position - bone1.position;
			normal.Normalize();
			
			float turnType = Random.value * totalTurnProbability;
			if(turnType <= harshTurnProbability)
			{
				float normAngle = Random.Range(0, 360);
				float vertAngle = Random.Range(Mathf.PI/6, Mathf.PI/4);
				
				Vector3 seg2Vec = GetArc(normal, vertAngle, normAngle);
				bone2.rotation = Quaternion.LookRotation(seg2Vec);
				
				float segment2Length = Vector3.Distance(bone3.localPosition, new Vector3(0,0,0));
				bone3.position = bone2.position + seg2Vec*segment2Length;
				
				Vector3 seg3Vec = GetArc(seg2Vec, vertAngle, normAngle);
				bone3.rotation = Quaternion.LookRotation(seg3Vec);
				
				float seg3Length = Vector3.Distance(boneEnd.localPosition, new Vector3(0,0,0));
				boneEnd.position = bone3.position + seg3Vec*seg3Length;
				boneEnd.rotation = bone3.rotation;
			}
			else if(turnType <= sTurnProbability)
			{
				float normAngle = Random.Range(0, 360);
				float vertAngle = Random.Range(Mathf.PI/6, Mathf.PI/4);
				
				Vector3 seg2Vec = GetArc(normal, vertAngle, normAngle);
				bone2.rotation = Quaternion.LookRotation(seg2Vec);
				
				float segment2Length = Vector3.Distance(bone3.localPosition, new Vector3(0,0,0));
				bone3.position = bone2.position + seg2Vec*segment2Length;
				
				Vector3 seg3Vec = GetArc(seg2Vec, vertAngle, Mathf.Repeat(normAngle+180,360));
				bone3.rotation = Quaternion.LookRotation(seg3Vec);
				
				float seg3Length = Vector3.Distance(boneEnd.localPosition, new Vector3(0,0,0));
				boneEnd.position = bone3.position + seg3Vec*seg3Length;
				boneEnd.rotation = bone3.rotation;
			}
			else // softTurnProbability
			{
				float normAngle = Random.Range(0, 360);
				float vertAngle = Random.Range(0, Mathf.PI/6);
				
				Vector3 seg2Vec = GetArc(normal, vertAngle, normAngle);
				bone2.rotation = Quaternion.LookRotation(seg2Vec);
				
				float segment2Length = Vector3.Distance(bone3.localPosition, new Vector3(0,0,0));
				bone3.position = bone2.position + seg2Vec*segment2Length;
				
				Vector3 seg3Vec = GetArc(seg2Vec, vertAngle, normAngle);
				bone3.rotation = Quaternion.LookRotation(seg3Vec);
				
				float seg3Length = Vector3.Distance(boneEnd.localPosition, new Vector3(0,0,0));
				boneEnd.position = bone3.position + seg3Vec*seg3Length;
				boneEnd.rotation = bone3.rotation;
			}
		}
		
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
