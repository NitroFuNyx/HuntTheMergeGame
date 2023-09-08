using System;
using UnityEngine;

public class ResourceManager : Singleton<ResourceManager>, IDataPersistance
{
    private DataPersistanceManager _dataPersistanceManager;
    [SerializeField] private GameData gameData;

    #region Events Declaration

    public event Action<int> OnMeatAmountChanged;

    #endregion Events Declaration

    private void Start()
    {
        _dataPersistanceManager = DataPersistanceManager.Instance;
        _dataPersistanceManager.AddObjectToSaveSystemObjectsList(this);
        gameData = new GameData();
    }

    #region Save/Load Methods

    public void LoadData(GameData data)
    {
        gameData.meatAmount = data.meatAmount;
        OnMeatAmountChanged?.Invoke(data.meatAmount.GetValue());
    }

    public void SaveData(GameData data)
    {
        data.meatAmount = new Secureint(gameData.meatAmount.GetValue());
    }

    #endregion Save/Load Methods

    #region Basic Resources Methods

    public void IncreaseMeatAmount(int deltaAmount)
    {
        gameData.meatAmount += new Secureint(deltaAmount);
        _dataPersistanceManager.SaveGame();
        OnMeatAmountChanged?.Invoke(gameData.meatAmount.GetValue());
    }

    public void DecreaseMeatAmount(int deltaAmount)
    {
        gameData.meatAmount -= new Secureint(deltaAmount);

        if (gameData.meatAmount.GetValue() < 0) gameData.meatAmount = new Secureint(0);
        _dataPersistanceManager.SaveGame();
        OnMeatAmountChanged?.Invoke(gameData.meatAmount.GetValue());
    }

    #endregion Basic Resources Methods

    // Start Of Test Methods
    [ContextMenu("Increase Resources Amount")]
    public void TestMethod_IncreaseResourcesAmount()
    {
        IncreaseMeatAmount(1000);
    }
    // End Of Test Methods

    public bool CheckIfEnoughResources(int cost)
    {
        var isEnoughResources = gameData.meatAmount.GetValue() >= cost;

        return isEnoughResources;
    }
}