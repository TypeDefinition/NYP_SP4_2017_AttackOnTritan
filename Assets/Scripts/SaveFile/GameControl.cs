using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GameControl : MonoBehaviour {

    public static GameControl control;

    public int[] Score;
    public int currlevel;
    public int maxlevel;
    

	void Awake ()
    {
        if (control == null)
        {
            DontDestroyOnLoad(gameObject);
            control = this;
        }
        else if (control != this)
        {
            Destroy(gameObject);
        }
        levelcheck();
        Debug.Log(maxlevel);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Save();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            Load();
        }
    }
    public void Save()
    {
        
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerSave.dat");

        PlayerData data = new PlayerData();
        data.Score[currlevel] = Score[currlevel];
        
        bf.Serialize(file, data);
        file.Close();
    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/playerSave.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerSave.dat",FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();

            Score = data.Score;
        }
    }

    private void levelcheck()
    {
        maxlevel = 0;
        for (int i = 0; i < Score.Length; i++)
        {
            if (Score[i] > 0 && Score[i] <= 300)
            {
                ++maxlevel;
            }
        }
    }
}

[Serializable]
class PlayerData
{
    public int[] Score;
}