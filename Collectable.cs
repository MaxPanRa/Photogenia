using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour, IDataPersistence
{
    public string playerTag = "Player";

    private bool collected = false;

    [SerializeField] private string id;
    [ContextMenu("Generar GUID")]
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }

    void Start()
    {
        if (id == "")
        {
            id = gameObject.GetInstanceID().ToString();
        }
        if (collected)
        {
            this.gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision c)
    {
        Debug.Log("COLLITION: " + c.gameObject.tag);
        if (c.gameObject.CompareTag(playerTag))
        {
            if (!collected)
                Collect();
        }
    }

    private void Collect()
    {
        this.gameObject.SetActive(false);
        collected = true;
        //Sonido de recolectado;
    }

    public void LoadData(GameData data)
    {
        data.collectables.TryGetValue(id, out collected);
        if (collected)
        {
            this.gameObject.SetActive(false);
        }
        else
        {
            this.gameObject.SetActive(true);
        }
    }
    public void SaveData(GameData data)
    {
        if (data.collectables.ContainsKey(id))
        {
            data.collectables.Remove(id);
        }
        data.collectables.Add(id, collected);
    }
}
