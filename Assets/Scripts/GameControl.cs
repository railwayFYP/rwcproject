using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GameControl : MonoBehaviour {

	public static GameControl control;

	public float numOfMissionUnlocked;
	public float numOfTimeAttackUnlocked;
	public float numOfCasualUnlocked;
	
	void Awake () 
	{
		if (control == null) 
		{
			DontDestroyOnLoad (gameObject);
			control = this;
		} 
		else if (control != this) 
		{
			Destroy(gameObject);
		}

		GameControl.control.Load ();
	}

	void OnGUI()
	{
		GUI.Label(new Rect(10, 10, 200, 30), "Num Of Mission Unlocked: " + numOfMissionUnlocked);
		GUI.Label(new Rect(10, 40, 200, 30), "Num Of Time Attack Unlocked: " + numOfTimeAttackUnlocked);
		GUI.Label(new Rect(10, 70, 200, 30), "Num Of Casual Unlocked: " + numOfCasualUnlocked);
	}

	void OnApplicationQuit()
	{
		GameControl.control.Save ();
	}

	public void Save()
	{
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "/playerInfo.dat");

		PlayerData data = new PlayerData ();
		data.numOfMissionUnlocked = numOfMissionUnlocked;
		data.numOfTimeAttackUnlocked = numOfTimeAttackUnlocked;
		data.numOfCasualUnlocked = numOfCasualUnlocked;

		bf.Serialize (file, data);
		file.Close ();
	}

	public void Load()
	{
		if (File.Exists (Application.persistentDataPath + "/playerInfo.dat")) 
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
			PlayerData data = (PlayerData)bf.Deserialize(file);
			file.Close ();

			numOfMissionUnlocked = data.numOfMissionUnlocked;
			numOfTimeAttackUnlocked = data.numOfTimeAttackUnlocked;
			numOfCasualUnlocked = data.numOfCasualUnlocked;
		}
	}
}

[Serializable]
class PlayerData
{
	public float numOfMissionUnlocked;
	public float numOfTimeAttackUnlocked;
	public float numOfCasualUnlocked;
}