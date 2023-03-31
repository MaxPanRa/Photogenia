using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class GameData
{
    public int playerWorld;
    public Vector3 position;
    public Quaternion rotation;
    public SerializableDictionary<string, bool> collectables;

    public GameData()
    {
        this.playerWorld = 0;
        this.position = new Vector3(0, -100, 0);
        this.rotation = new Quaternion(0, -100, 0,0);
        this.collectables = new SerializableDictionary<string, bool>();
    }
}
