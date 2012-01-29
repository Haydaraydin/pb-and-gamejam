using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour 
{
	
    public GUIStyle invisibleButton;
	
	// Use this for initialization
	void Start () 
	{
		Time.timeScale = 1.0f;
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	void OnGUI()
	{
        if (GUI.Button(new Rect(0, 300, 400, 200), "", invisibleButton))
        {
            Application.LoadLevel(1);
        }
	}
}
