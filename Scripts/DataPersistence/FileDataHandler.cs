using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";

    private bool useEncryption = false;
    private readonly string codeWord = "mayahuel";

    public FileDataHandler(string dataDirPath, string dataFileName, bool useEncryption){
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
        this.useEncryption = useEncryption;
    }

    public GameData Load()
    {
        // use Path.Combine to account for different OS's having different path separators
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        GameData loadedData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                //Cargar la data serializada del archivo
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using(StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                //deserializar los datos de jjson de vuelta a C#
                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("Error ocurred when trying to load data from file: " + fullPath + "\n"+e);
            }
        }
        return loadedData;
    }

    public void Save(GameData data)
    {
        // use Path.Combine to account for different OS's having different path separators
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        try
        {
            //Crear el directorio del archivo si este no existe
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            //Serializar los datos de C# a Json
            string dataToStore = JsonUtility.ToJson(data, true);
            //Encriptamos datos
            if (useEncryption)
            {
                dataToStore = EncryptDecrypt(dataToStore);
            }
            //Escribir los datos serializados al archivo
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }

        } 
        catch (Exception e)
        {
            Debug.LogError("Error Ocurred when trying to save data to file: " + fullPath + "\n" + e);
        }
    }

    private string EncryptDecrypt (string data)
    {
        string modifiedData = "";
        for(int i = 0; i < data.Length; i++)
        {
            modifiedData += (char)(data[i] ^ codeWord[i % codeWord.Length]);
        }
        return modifiedData;
    }
}
