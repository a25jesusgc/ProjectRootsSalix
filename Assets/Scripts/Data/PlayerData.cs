using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class PlayerData
{
    [SerializeField] private static PlayerData instance;
    
    public static PlayerData GetInstance => instance != null ? instance : Load();
    public static void ResetInstance() => instance = new PlayerData();

    [SerializeField] private Vector3 respawnPoint;
    [SerializeField] private List<int> availableWeapons;
    [SerializeField] private List<string> itemsPicked;
    [SerializeField] private int lifeUpgrades;

    public Vector3 GetRespawn => respawnPoint;
    public void SetRepawnPoint(Vector3 value) => respawnPoint = value;
    
    public List<int> GetAvailableWeapons => availableWeapons;
    public void UnlockWeapon(int index) => availableWeapons.Add(index);
    
    public bool WasItemPicked(string id) => itemsPicked.Contains(id);
    public void PickItem(string id) => itemsPicked.Add(id);
    
    public int GetLifeUpgrades => lifeUpgrades;
    public void GotLifeUpgrade() => lifeUpgrades++;

    public PlayerData()
    {
        respawnPoint = Vector3.zero;

        // Empieza con las tres primeras armas desbloqueadas
        availableWeapons = new List<int>() {0, 1, 2};

        itemsPicked = new List<string>();

        itemsPicked = new List<string>();

        lifeUpgrades = 0;
    }

    // Función para guardar los datos en un archivo de guardado
    public static void Save()
    {
        string data = JsonUtility.ToJson(instance);
        File.WriteAllText(GlobalUtils.SAVE_PATH, EncryptData(data));
    }

    // Función para cargar los datos o crear datos nuevos si no existe el archivo de guardado
    public static PlayerData Load()
    {
        string filePath = GlobalUtils.SAVE_PATH;
        if (File.Exists(filePath))
        {
            try
            {
                string data = DecryptData(File.ReadAllText(filePath));

                PlayerData playerData = JsonUtility.FromJson<PlayerData>(data);

                instance = playerData;
                return instance;
            }
            catch (IOException e)
            {
                Debug.Log(e);
                instance = new PlayerData();
                return instance;
            }
        }
        else
        {
            instance = new PlayerData();
            return instance;
        }
    }

    public static string EncryptData(string data)
    {
        return data;
        //return Convert.ToBase64String(Encoding.UTF8.GetBytes(data));
    }
    public static string DecryptData(string data)
    {
        return data;
        //return Encoding.UTF8.GetString(Convert.FromBase64String(data));
    }

}
