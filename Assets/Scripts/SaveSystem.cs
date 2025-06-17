using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

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
            // StartCoroutine(AutosaveCoroutine());
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
            currentPlayerData.position = new Vector3(-3, 1, 0);
        }
    }

    public void Save()
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
                if (!(SceneManager.GetActiveScene().name == "Lab"))
                {
                    //overworld, set player position
                    GameObject player = GameObject.FindGameObjectWithTag("Player");
                    player.transform.position = currentPlayerData.position;
                }
            }
        }
        else
        {
            Debug.LogError("Save file not found!");
        }
    }

    IEnumerator Autosave()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(180f);
            Save();
        }
    }
}
