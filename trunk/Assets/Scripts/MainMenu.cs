using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour 
{
    public GUIStyle invisibleButton;
	
	// Use this for initialization
	void Start () 
	{
		Time.timeScale = 1.0f;
		StaticGameProperties.GetInstance().SetLevelToStartAt(0);
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

public class StaticGameProperties : MonoBehaviour
{
	
	private static int levelToStartAt = 0;
	
	private static StaticGameProperties instance;
	
	void Start()
	{
		DontDestroyOnLoad(this);
	}
	
	void Update()
	{
	}
	
	public static StaticGameProperties GetInstance()
	{
		if (instance == null)
			instance = new StaticGameProperties();
		return instance;
	}
	
	public void SetLevelToStartAt(int val)
	{
		levelToStartAt = val;
	}
	
	public int GetLevelToStartAt()
	{
		return levelToStartAt;
	}
}
