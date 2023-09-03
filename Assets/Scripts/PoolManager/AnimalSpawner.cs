using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class AnimalSpawner : MonoBehaviour
{
    [Header("Holders")]
    [Space]
    [SerializeField] private Transform spawnedObjectsHolder;
    [SerializeField] private List<TileHolder> tilesList;

    [Header("VFX")]
    [Space]
    [SerializeField] private List<ParticleSystem> vfxSpawnList = new List<ParticleSystem>();

    [SerializeField] private PoolItemsManager _poolItemsManager;
    [SerializeField] private TileInteractionHandler tileInteractionHandler;
    
   
    public void UpdateTileList(TileHolder tile)
    {

      //  if (tile == null) return;
      if(tile.MViewModel.IsOccupied)
            tilesList.Remove(tile);
      else if(tilesList.Contains(tile))
      {
          return;
          
      }
      else
      {
          tilesList.Add(tile);
      }
    }

    public void SpawnAnimal()
    {
        if (tilesList.Count <= 0) return;
        ResourceManager.Instance.DecreaseMeatAmount(20);
        var targetTile = tilesList[(int)Random.Range(0,tilesList.Count)];
        if(targetTile.transform != null)
        {       
            tileInteractionHandler.OccupyTile(targetTile,true,1);
            PoolItem poolItem = _poolItemsManager.SpawnItemFromPool(PoolItemsTypes.RedFox, targetTile.transform.position, Quaternion.identity, spawnedObjectsHolder);
            
            if (poolItem != null)
            {
                poolItem.SetObjectAwakeState();
                
            }
            else
            {
                Debug.LogWarning($"There is no {PoolItemsTypes.RedFox} models left in the pool to spawn at {gameObject}", gameObject);
            }
        }
    }
    public void SpawnAnimal(PoolItemsTypes type,TileHolder targetTile)
    {
        if(targetTile.transform != null)
        {       
            tileInteractionHandler.OccupyTile(targetTile,true,(int)type);

            PoolItem poolItem = _poolItemsManager.SpawnItemFromPool(type, targetTile.transform.position, Quaternion.identity, spawnedObjectsHolder);
            
            if (poolItem != null)
            {
                poolItem.SetObjectAwakeState();
            }
            else
            {
                Debug.LogWarning($"There is no {type} models left in the pool to spawn at {gameObject}", gameObject);
            }
        }
    }
    public void ReturnAnimalToPool(PoolItem item)
    {
        item.PoolItemsManager.ReturnItemToPool(item);
    }

    public void ReturnAllAnimalsToPool()
    {
        List<PoolItem> poolItemsList = new List<PoolItem>();
        
        for(int i = 0; i < spawnedObjectsHolder.childCount; i++)
        {
            if(spawnedObjectsHolder.GetChild(i).TryGetComponent(out PoolItem poolItem))
            {
                poolItemsList.Add(poolItem);
            }
        }

        for(int i = 0; i < poolItemsList.Count; i++)
        {
            poolItemsList[i].PoolItemsManager.ReturnItemToPool(poolItemsList[i]);
        }

       
    }
    

    private void ActivateSpawnVFX()
    {
        if(vfxSpawnList.Count > 0)
        {
            for (int i = 0; i < vfxSpawnList.Count; i++)
            {
                vfxSpawnList[i].Play();
            }
        }
    }
    

  
}
