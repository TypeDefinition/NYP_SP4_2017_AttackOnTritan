using UnityEngine;
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
        Load();
        levelcheck();
        Debug.Log(maxlevel);
    }

    public void Save(int score)
    {
        
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerSave.dat");

        PlayerData data = new PlayerData();
        int[] temp = new int[currlevel + 1];
        Array.Copy(Score, temp, Score.Length);
        
        data.Score = temp;
        if (score >= data.Score[currlevel])
        {
            data.Score[currlevel] = score;
            Score = data.Score;
            levelcheck();
        }

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
            levelcheck();
        }
    }

    private void levelcheck()
    {
        maxlevel = 0;
        Debug.Log(Score.Length);
        for (int i = 0; i < Score.Length; i++)
        {
            if (Score[i] > 0 && Score[i] <= 5000)
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