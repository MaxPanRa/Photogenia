using UnityEngine;
using SimpleJSON;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SavingSystem{

    public static void saveState(string jsonString, string filename)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string savePath = Application.persistentDataPath + "/"+ filename;
        FileStream stream = new FileStream(savePath, FileMode.Create);
        DataString data = new DataString(jsonString);
        formatter.Serialize(stream,data);
        stream.Close();
        //Debug.Log("Data is Saved");
    }

    public static string loadState(string filename)
    {
        string savePath = Application.persistentDataPath + "/" + filename;
        if (File.Exists(savePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(savePath, FileMode.Open);
            DataString data = formatter.Deserialize(stream) as DataString;
            stream.Close();
            Debug.Log(filename+ " Loaded");
            //Debug.Log(data.jsonString);
            return data.jsonString; 
        }
        else
        {
            //Debug.Log(filename + " Not Loaded");
            return null;
        }
        
    }

}

#region DATA STRING REGION

[System.Serializable]
public class DataString{
    public string jsonString;

    public DataString(string dataString)
    {
        jsonString = dataString;
    }
}

#endregion