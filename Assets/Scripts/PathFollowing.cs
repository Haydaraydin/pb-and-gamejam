using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathFollowing : MonoBehaviour 
{
	public Vector3 initialPosition;
	public float defaultMoveSpeed = 1.0f;
	private float moveSpeed;
	public bool loop = false;
	public float maxDistance = 5.0f;
	
	private float currPathPosition;
	private float pathEnd;
	
	private Vector3 desiredPosition;
	private Vector3 dampingVelocity;
	private float smoothTime = 0.3f;
	
	private Quaternion prevQuat;
	private Quaternion nextQuat;
	private float quatBlendTimer;
	private float totQuatBlend = 0.3f;
	
	public GameObject tunnelSpawner;
	
	public class SubSegment
	{
		public float startDistance;
		public float endDistance;
		public Vector3 startPoint;
		public Vector3 endPoint;
	}
	
	public class Segment
	{
		public float startDistance;
		public float endDistance;
		public SubSegment[] subSegments;
	}
	
	private List<Segment> segments;
	
	public bool debugVerifySegments = true;
	
	public float GetPathPosition()
	{
		return currPathPosition;
	}
	
	public float GetPathEnd()
	{
		return pathEnd;
	}
	
	void Awake ()
	{
		segments = new List<Segment>();
	}
	
	// Use this for initialization
	void Start () 
	{
		currPathPosition = 0.0f;
		pathEnd = 0.0f;
		transform.position = initialPosition;
		moveSpeed = defaultMoveSpeed;
		
		prevQuat = Quaternion.identity;
		nextQuat = Quaternion.identity;
		quatBlendTimer = 0.0f;
	}
	
	// Update is called once per frame
	void Update () 
	{
		currPathPosition += moveSpeed*Time.deltaTime;
		
//		if(loop && (newPos.z > maxDistance + initialPosition.z))
//		{
//			transform.position = initialPosition;
//			
//			tunnelSpawner.GetComponent<TunnelSpawnerScript>().Reset();
//		}
//		else
//		{
		Vector3 point = new Vector3(0,0,0);
		Vector3 normal = new Vector3(0,0,0);
		GetPointAndNormalAt(currPathPosition, ref point, ref normal);
		
		desiredPosition = point;
		
		transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref dampingVelocity, smoothTime);
		
		Quaternion quat = Quaternion.LookRotation(normal);
		if(nextQuat == Quaternion.identity)
		{
			prevQuat = quat;
			nextQuat = quat;
			transform.rotation = quat;
		}
		else if(quat != nextQuat)
		{
			nextQuat = quat;
			prevQuat = transform.rotation;
			quatBlendTimer = totQuatBlend;
		}
		else if(quatBlendTimer > 0.0f)
		{
			transform.rotation = Quaternion.Slerp(nextQuat, prevQuat, quatBlendTimer/totQuatBlend);
			quatBlendTimer -= Time.deltaTime;
		}
//		}
		
	}
	
	void OnGUI()
	{
		//GUILayout.Label("\n\n\n\n\n\nCurrent Position: " + transform.position);
	}
	
	public void SetMoveSpeed(float newSpeed)
	{
		moveSpeed = newSpeed;
	}
	
	public float GetMoveSpeed()
	{
		return moveSpeed;
	}
	
	public void AddSegment(Segment segmentToAdd)
	{
		if(debugVerifySegments)
		{
			if(!VerifySegment(segmentToAdd))
				throw new System.Exception("Trying to Add Invalid Segment!");
		}
		
		segments.Add(segmentToAdd);
		
		pathEnd = segmentToAdd.endDistance;
	}
	
	public bool VerifySegment(Segment segmentToAdd)
	{
		// Verify Segment is good.
		Vector3 lastPoint;
		Vector3 lastDirection;
		float lastDistance;
	
		if(segments.Count == 0)
		{
			lastDistance = -5.0f;
			lastPoint = segmentToAdd.subSegments[0].startPoint;
			lastDirection = new Vector3(0.0f, 0.0f, 1.0f);
		}
		else
		{
			Segment lastSegment = segments[segments.Count-1];
			lastDistance = lastSegment.endDistance;
			
			SubSegment lastSubSegment = segments[segments.Count-1].subSegments[lastSegment.subSegments.Length-1];
			lastPoint = lastSubSegment.endPoint;
			lastDirection = lastPoint - lastSubSegment.startPoint;
			lastDirection.Normalize();
		}
		
		if(lastDistance != segmentToAdd.startDistance)
			return false;
		
		for(int i = 0; i < segmentToAdd.subSegments.Length; ++i)
		{
			SubSegment sub = segmentToAdd.subSegments[i];
			
			if(lastDistance != sub.startDistance)
				return false;
			
			if(lastPoint != sub.startPoint)
				return false;
			
			Vector3 direction = sub.endPoint - sub.startPoint;
			direction.Normalize();
			if(i == 0 && lastDirection != direction)
				return false;
			
			lastDistance = sub.endDistance;
			lastPoint = sub.endPoint;
		}
		
		if(lastDistance != segmentToAdd.endDistance)
			return false;
		
		return true;
	}
	
	public void RemoveSegment(Segment segmentToRemove)
	{
		int index = segments.IndexOf(segmentToRemove);
		
		if(debugVerifySegments)
		{
			if(index != 0)
				throw new System.Exception("Trying to Remove Segment Other than First!");
		}
		
		segments.RemoveAt(index);
	}
	
	public void GetPointAndNormalAt(float distAlongPath, ref Vector3 point, ref Vector3 normal)
	{
		int index = SearchSegments(distAlongPath, 0, segments.Count-1);
		
		if(index < 0)
		{
			if(segments.Count > 0)
				throw new System.Exception("Error getting Point in Path.");
			
			point = new Vector3(0.0f, 0.0f, 0.0f);
			normal = new Vector3(0.0f, 0.0f, 1.0f);
			
			return;
		}
		
		Segment segment = segments[index];
		
		foreach(SubSegment sub in segment.subSegments)
		{
			if(distAlongPath >= sub.startDistance 
			   && distAlongPath <= sub.endDistance)
			{
				float ratio = (distAlongPath - sub.startDistance)/(sub.endDistance - sub.startDistance);
				
				point = Vector3.Lerp(sub.startPoint, sub.endPoint, ratio);
				normal = sub.endPoint - sub.startPoint;
				normal.Normalize();
			}
		}
	}
		
	private int SearchSegments(float desiredValue, int low, int high)
	{
		if(high < low)
			return -1;

		int mid = (low + high) /2;
		
		if( desiredValue > segments[mid].endDistance )
			return SearchSegments(desiredValue, mid+1, high);
		else if( desiredValue < segments[mid].startDistance )
			return SearchSegments(desiredValue, low, mid-1);
		else
			return mid;
	}
}
