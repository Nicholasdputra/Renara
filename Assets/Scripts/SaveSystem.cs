using UnityEngine;
using System.Collections.Generic;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem currentSave;
    public PlayerDataSO currentPlayerData;

    // Start is called before the first frame update
    void Awake()
    {
        if (currentSave == null)
        {
            currentSave = this;
            DontDestroyOnLoad(gameObject);
            InitializePlayerData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializePlayerData()
    {
        if (currentPlayerData == null)
        {
            currentPlayerData = ScriptableObject.CreateInstance<PlayerDataSO>();
            currentPlayerData.obtainedMaterials = new List<CraftingMaterial>();
            currentPlayerData.position = Vector3.zero;
        }
    }

    void Save()
    {
        string json = JsonUtility.ToJson(currentPlayerData);
        Debug.Log(json);

        using (System.IO.StreamWriter writer = new System.IO.StreamWriter(Application.persistentDataPath + "/save.json"))
        {
            writer.Write(json);
        }
    }

    void Load()
    {
        if (System.IO.File.Exists(Application.persistentDataPath + "/save.json"))
        {
            using (System.IO.StreamReader reader = new System.IO.StreamReader(Application.persistentDataPath + "/save.json"))
            {
                string json = reader.ReadToEnd();
                currentPlayerData = JsonUtility.FromJson<PlayerDataSO>(json);
            }
        }
        else
        {
            Debug.LogError("Save file not found!");
        }
    }
}
