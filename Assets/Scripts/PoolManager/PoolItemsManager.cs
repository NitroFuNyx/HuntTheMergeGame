using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolItemsManager : MonoBehaviour
{
    [Header("Pool Data")] [Space] [SerializeField]
    private int redFoxesPoolSize = 15;

    [SerializeField] private int grayFoxesPoolSize = 14;
    [SerializeField] private int wolfsPoolSize = 12;

    [Header("Active Pools")] [Space] [SerializeField]
    private readonly List<List<PoolItem>> activePoolsList = new();

    [Header("Prefabs")] [Space] [SerializeField]
    private PoolItem redFoxesPrefab;

    [SerializeField] private PoolItem grayFoxesPrefab;
    [SerializeField] private PoolItem wolfsPrefab;

    private readonly Dictionary<PoolItemsTypes, List<PoolItem>> itemsListsDictionary =
        new();

    private readonly Dictionary<PoolItemsTypes, Transform> itemsHoldersDictionary = new();

    private readonly Vector3 poolItemsSpawnPos = new(100f, 100f, 100f);


    private void Start()
    {
        CreatePool(redFoxesPrefab, "Red fox", redFoxesPoolSize);
        CreatePool(grayFoxesPrefab, "Gray fox", grayFoxesPoolSize);
        CreatePool(wolfsPrefab, "Wolf", wolfsPoolSize);
    }

    public PoolItem SpawnItemFromPool(PoolItemsTypes poolItemType, Vector3 _spawnPos, Quaternion _rotation,
        Transform _parent)
    {
        PoolItem poolItem = null;

        if (itemsListsDictionary.ContainsKey(poolItemType))
        {
            var poolItemsList = itemsListsDictionary[poolItemType];

            for (var i = 0; i < poolItemsList.Count; i++)
                if (!poolItemsList[i].gameObject.activeInHierarchy)
                {
                    poolItem = poolItemsList[i];
                    break;
                }

            if (poolItem != null)
            {
                poolItem.transform.SetParent(_parent);
                poolItem.transform.position = _spawnPos;
                poolItem.transform.rotation = _rotation;
                poolItem.gameObject.SetActive(true);
                poolItemsList.Remove(poolItem);
            }
        }
        else
        {
            Debug.LogError($"No pool for such prefab: {poolItemType} in dictionary");
        }


        return poolItem;
    }

    public void ReturnItemToPool(PoolItem _poolItem)
    {
        var poolItemsList = itemsListsDictionary[_poolItem.PoolItemType];

        _poolItem.gameObject.SetActive(false);
        _poolItem.transform.SetParent(itemsHoldersDictionary[_poolItem.PoolItemType]);
        _poolItem.transform.localPosition = Vector3.zero;
        _poolItem.ResetPoolItem();
        poolItemsList.Add(_poolItem);
    }

    private void CreatePool(PoolItem poolItemPrefab, string itemName, int poolSize)
    {
        var poolItemsParent = new GameObject();
        poolItemsParent.transform.SetParent(transform);
        poolItemsParent.name = $"{itemName} Items Parent";
        poolItemsParent.transform.position = poolItemsSpawnPos;

        var itemsList = new List<PoolItem>();

        activePoolsList.Add(itemsList);

        itemsListsDictionary.Add(poolItemPrefab.PoolItemType, itemsList);
        itemsHoldersDictionary.Add(poolItemPrefab.PoolItemType, poolItemsParent.transform);

        for (var i = 0; i < poolSize; i++)
        {
            var poolItem =
                Instantiate(poolItemPrefab, Vector3.zero, Quaternion.identity, poolItemsParent.transform);
            poolItem.transform.localPosition = Vector3.zero;
            poolItem.CashComponents(this);
            itemsList.Add(poolItem);
            poolItem.name = $"{itemName} {i}";
        }

        StartCoroutine(TurnNewPoolItemsOffCoroutine(itemsList));
    }

    private IEnumerator TurnNewPoolItemsOffCoroutine(List<PoolItem> itemsList)
    {
        yield return null;

        for (var i = 0; i < itemsList.Count; i++) itemsList[i].gameObject.SetActive(false);
    }
}