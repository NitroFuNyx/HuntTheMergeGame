using UnityEngine;
using System;

public class PoolItem : MonoBehaviour
{
    [Header("Item Data")]
    [Space]
    [SerializeField] private PoolItemsTypes poolItemType;

    private PoolItemsManager _poolItemsManager;
    public PoolItemsTypes PoolItemType { get => poolItemType;}
    public PoolItemsManager PoolItemsManager { get => _poolItemsManager; private set => _poolItemsManager = value; }




    #region Events Declaration
    public event Action OnItemResetRequired;
    public event Action OnObjectAwakeStateSet;
    #endregion Events Declaration

    public void ResetPoolItem()
    {
        OnItemResetRequired?.Invoke();
    }

    public void SetObjectAwakeState()
    {
        OnObjectAwakeStateSet?.Invoke();
    }

    public void CashComponents()
    {
        
    }
}