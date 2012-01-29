using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour 
{
    public GUIStyle invisibleButton;
	public Texture menuTexture;
	public GUIStyle style;
	public GUIStyle titleStyle;
	
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
		GUI.DrawTexture(new Rect(0,0,1280,800), menuTexture);
		
		GUI.Label(new Rect(50,50,900,500), "Constricted\n  Cheddar Chaser", titleStyle);
		
		GUI.Label(new Rect(172,305,300,100), "Start game (Progression)", style);
		GUI.Label(new Rect(172,380,300,100), "Start game (Hard mode)", style);
		
		GUI.Label(new Rect(260,565,300,100), "A game by:", style);
		GUI.Label(new Rect(260,615,700,200), 
			"Lawrence Collier - Technical Artist\nAmanda Leduc - Sound Artist\nFrancis Petrin - Programmer\nPat Lalonde - Programmer", style);
		
        if (GUI.Button(new Rect(0, 275, 400, 100), "", invisibleButton))
        {
			StaticGameProperties.GetInstance().SetLevelToStartAt(0);
            Application.LoadLevel(1);
        }
		
        if (GUI.Button(new Rect(0, 375, 400, 100), "", invisibleButton))
        {
			StaticGameProperties.GetInstance().SetLevelToStartAt(7);
            Application.LoadLevel(1);
        }
	}
}

public class StaticGameProperties
{
	
	private static int levelToStartAt = 0;
	
	private static StaticGameProperties instance;
	
	
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
