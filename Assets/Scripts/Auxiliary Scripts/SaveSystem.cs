using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem currentSave;
    public PlayerDataSO currentPlayerData;
    [SerializeField] private PlayerDataSO playerDataTemplate;

    // Start is called before the first frame update
    void Awake()
    {
        Debug.Log(Application.persistentDataPath + "/save.json");
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
        StartCoroutine(Autosave());
    }

    private void InitializePlayerData()
    {
        currentPlayerData = playerDataTemplate;
        if (!System.IO.File.Exists(Application.persistentDataPath + "/save.json"))
        {
            Debug.Log("No save file found, creating a new one.");
            currentPlayerData.obtainedItemDataSO.items = new List<ItemSO>();
            foreach (PlantSO plant in currentPlayerData.plantDataSO.plant)
            {
                plant.isUnlocked = false;
            }
            currentPlayerData.obtainedMaterials = new List<CraftingMaterial>();
            currentPlayerData.position = new Vector3(-3, 1, 0);
            currentPlayerData.currentPlant = -1;
            currentPlayerData.playerToolLevel = 0;
            currentPlayerData.hasCuredZone1 = false;
            currentPlayerData.hasCuredZone2 = false;
            currentPlayerData.hasCuredZone3 = false;
            currentPlayerData.hasCuredZone4 = false;
            Save();
        }
        else
        {
            // Debug.Log("Theres a json file, loading it.");
            // Load();
        }
    }

    public void Save()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        currentPlayerData.position = player.transform.position;
        string json = JsonUtility.ToJson(currentPlayerData);
        Debug.Log(json);

        using (System.IO.StreamWriter writer = new System.IO.StreamWriter(Application.persistentDataPath + "/save.json"))
        {
            writer.Write(json);
        }
    }

    void Load()
    {
        Debug.Log("Has Json: "+ System.IO.File.Exists(Application.persistentDataPath + "/save.json"));
        using (System.IO.StreamReader reader = new System.IO.StreamReader(Application.persistentDataPath + "/save.json"))
        {
            string json = reader.ReadToEnd();
            Debug.Log(json);
            currentPlayerData = JsonUtility.FromJson<PlayerDataSO>(json);
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
