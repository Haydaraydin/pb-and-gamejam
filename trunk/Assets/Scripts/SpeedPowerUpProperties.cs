using UnityEngine;
using System.Collections;

public class SpeedPowerUpProperties : MonoBehaviour 
{
	
	public float moveSpeedValue = 0.3f;
	public float duration = 3.0f;
	
	private bool destroyOnNextUpdate = false;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (destroyOnNextUpdate)
			Destroy(gameObject);
	}
	
	public void DestroyOnNextUpdate()
	{
		destroyOnNextUpdate = true;
	}
}
