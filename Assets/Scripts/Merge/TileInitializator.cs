using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TileInitializator : MonoBehaviour,IDataPersistance
{
    [SerializeField] private List<TileHolder> tilesList;
    [SerializeField] private TileInteractionHandler tileInteractionHandler;
    [SerializeField] private AnimalSpawner animalSpawner;

    private void Start()
    {
        DataPersistanceManager.Instance.AddObjectToSaveSystemObjectsList(this);
        tileInteractionHandler.OnAvailableTilesListUpdated += UpdateAvailableLists;
    }

   

    private void UpdateAvailableLists(TileHolder tile)
    {
        animalSpawner.UpdateTileList(tile);

    }

    private void InitTiles()
    {
        for(int i=0;i<tilesList.Count;i++)
        {
            tilesList[i].MViewModel = new TileHolder.ViewModel
            {
                TileId = i,
                IsLocked = i<5?false:tilesList[i].MViewModel.IsLocked,
                IsOccupied = tilesList[i].MViewModel.IsOccupied,
                Level = tilesList[i].MViewModel.Level,
                
            };
            if (!tilesList[i].MViewModel.IsLocked&&!tilesList[i].MViewModel.IsOccupied)
            {
                UpdateAvailableLists(tilesList[i]);
            }

            if (tilesList[i].MViewModel.Level>0&&tilesList[i].MViewModel.Level<4)
            animalSpawner.SpawnAnimal((PoolItemsTypes)tilesList[i].MViewModel.Level,tilesList[i]);
        }
        tileInteractionHandler.InitTilesList(tilesList);
    }

    public void InitializeFirstTime(GameData data)
    {
        for (int i = 0; i < tilesList.Count; i++)
        {
            tilesList[i].MViewModel = new TileHolder.ViewModel
            {
                TileId = i,
                IsLocked = i >= 5
            };
            data.tilesList.Add(tilesList[i].MViewModel);   

        }
    }
    

    public void LoadData(GameData data)
    {
        for (int i = 0; i < data.tilesList.Count; i++)
        {
            tilesList[i].MViewModel= data.tilesList[i];

        }
        InitTiles();

    }

    public void SaveData(GameData data)
    {
        for (int i = 0; i < tilesList.Count; i++)
        {
            Debug.Log($"<color=yellow>{data.tilesList[i].IsLocked} and {tilesList[i].MViewModel.IsLocked}</color");
            data.tilesList[i]=tilesList[i].MViewModel;   

        }
    }
}
