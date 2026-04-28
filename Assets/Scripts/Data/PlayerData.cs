using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

[Serializable]
public class PlayerData
{
    [SerializeField] private static PlayerData instance;
    
    public static PlayerData GetInstance => instance != null ? instance : Load();
    public static void ResetInstance() => instance = new PlayerData();

    [SerializeField] private string checkpointID;
    [SerializeField] private List<int> availableWeapons;
    [SerializeField] private List<string> itemsPicked;
    [SerializeField] private int currency;
    [SerializeField] private List<string> checkpointsDiscovered;
    [SerializeField] private int lifeUpgrades;
    [SerializeField] private List<PlayerFertilizer> playerFertilizers;
    [SerializeField] private float dayTime;
    [SerializeField] private List<EnemyDefeated> enemiesDefeated;

    public string GetCheckpointID => checkpointID;
    public void SetCheckpoint(string value) => checkpointID = value;
    public void SetCheckpoint(Checkpoint value) => checkpointID = value.GetZoneID;
    
    public List<int> GetAvailableWeapons => availableWeapons;
    public void UnlockWeapon(int index) => availableWeapons.Add(index);
    
    public bool WasItemPicked(string id) => itemsPicked.Contains(id);
    public void PickItem(string id) => itemsPicked.Add(id);

    public int GetCurrency => currency;
    public void ChangeCurrency(int amount) => currency += amount;
    
    public bool WasCheckpointDiscovered(Checkpoint checkpoint) => checkpointsDiscovered.Contains(checkpoint.GetZoneID);
    public void DiscoverCheckpoint(Checkpoint checkpoint) => checkpointsDiscovered.Add(checkpoint.GetZoneID);
    
    public int GetLifeUpgrades => lifeUpgrades;
    public void GotLifeUpgrade() => lifeUpgrades++;

    public List<PlayerFertilizer> GetPlayerFertilizers => playerFertilizers;
    
    public float GetDayTime => dayTime;
    public void SetDayTime(float value) => dayTime = value;

    public void AddFertilizer(PlayerFertilizer fertilizer)
    {
        if (playerFertilizers.Count((f) => f.GetFertilizerType == fertilizer.GetFertilizerType) > 0)
        {
            playerFertilizers.First((f) => f.GetFertilizerType == fertilizer.GetFertilizerType).ChangeAmount(fertilizer.GetFertilizerAmount);
        }
        else
        {
            playerFertilizers.Add(fertilizer);
        }
    }

    public bool UseFertilizer(PlayerFertilizer fertilizer)
    {
        fertilizer.ChangeAmount(-1);
        if (fertilizer.GetFertilizerAmount <= 0)
        {
            playerFertilizers.Remove(fertilizer);
            return true;
        }
        return false;
    }

    public void DefeatEnemy(string enemy)
    {
        if (enemiesDefeated.Count((e) => e.GetEnemyType == enemy) > 0)
        {
            enemiesDefeated.First((e) => e.GetEnemyType == enemy).DefeatEnemy();
        }
        else
        {
            enemiesDefeated.Add(new EnemyDefeated(enemy));
        }
    }

    public int GetEnemyDefeatCount(string enemy)
    {
        if(enemiesDefeated.Count((e) => e.GetEnemyType == enemy) == 0) return 0;
        return enemiesDefeated.First((e) => e.GetEnemyType == enemy).GetDefeatCount;
    }

    public PlayerData()
    {
        checkpointID = null;

        // Empieza con las tres primeras armas desbloqueadas
        availableWeapons = new List<int>() {0, 1, 2};

        itemsPicked = new List<string>();

        currency = 0;

        checkpointsDiscovered = new List<string>();

        lifeUpgrades = 0;

        playerFertilizers = new List<PlayerFertilizer>();

        dayTime = 600f;

        enemiesDefeated = new List<EnemyDefeated>();
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
