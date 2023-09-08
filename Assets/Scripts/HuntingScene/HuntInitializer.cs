using System.Collections.Generic;
using UnityEngine;

public class HuntInitializer : MonoBehaviour, IDataPersistance
{
    [SerializeField] private AnimalSpawner animalSpawner;

    [SerializeField] private List<TileHolder.ViewModel> tilesList;
    private void Start()
    {
        DataPersistanceManager.Instance.AddObjectToSaveSystemObjectsList(this);
    }
    private void InitTiles()
    {
        for (var i = 0; i < tilesList.Count; i++)
        {
            tilesList[i] = new TileHolder.ViewModel
            {
                TileId = i,
                IsLocked = i < 5 ? false : tilesList[i].IsLocked,
                IsOccupied = tilesList[i].IsOccupied,
                Level = tilesList[i].Level
            };
            if (tilesList[i].Level > 0 && tilesList[i].Level < 4&&tilesList[i].TileId>=0&&tilesList[i].TileId<=9)
            {
                animalSpawner.SpawnAnimal((PoolItemsTypes) tilesList[i].Level);
            }
        }
    }
    
    public void LoadData(GameData data)
    {
        for (var i = 0; i < data.tilesList.Count; i++)
            tilesList.Add(data.tilesList[i]);  
        InitTiles();
    }

    public void SaveData(GameData data)
    {
       
    }
}
