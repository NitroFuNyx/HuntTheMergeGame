using System;
using System.Collections.Generic;
using UnityEngine;

public class TileInteractionHandler : MonoBehaviour
{
    [SerializeField] private List<TileHolder> tilesList;
    [SerializeField] private AnimalSpawner animalSpawner;
    private ResourceManager _resourceManager;

    public event Action<TileHolder> OnAvailableTilesListUpdated;

    private void Start()
    {
        _resourceManager = ResourceManager.Instance;
    }

    public void InitTilesList(List<TileHolder> tiles)
    {
        tilesList = tiles;
    }

    public void BuyTile(TileHolder tile)
    {
        if (tile.MViewModel.IsLocked && _resourceManager.CheckIfEnoughResources(500))
        {
            _resourceManager.DecreaseMeatAmount(500);
            tile.UnBlockTile();
            Debug.Log($"1.bought tile {tile}", tile.gameObject);
            OnAvailableTilesListUpdated?.Invoke(tile);
            DataPersistanceManager.Instance.SaveGame();
        }
    }

    public void BuyAnimal()
    {
        if (_resourceManager.CheckIfEnoughResources(20) && animalSpawner.CheckAvailabilityOfSpawnPoints())
        {
            _resourceManager.DecreaseMeatAmount(20);
            animalSpawner.SpawnAnimal();
        }
    }

    public void OccupyTile(TileHolder tile, bool isOccupied, int level = 0)
    {
        if (tile == null) return;
        tile.OccupyTile(isOccupied, level);
        OnAvailableTilesListUpdated?.Invoke(tile);
        DataPersistanceManager.Instance.SaveGame();
    }
}