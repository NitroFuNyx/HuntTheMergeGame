using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPersistanceManager : Singleton<DataPersistanceManager>
{
    [SerializeField] private TileInitializator tileInitializator;
    private readonly float loadStartDataDelay = 0.1f;

    private readonly List<IDataPersistance> saveSystemDataObjectsList = new();

    
    private GameData gameData;

    private void Start()
    {
        StartCoroutine(LoadStartDataCoroutine());
    }


    public void NewGame()
    {
        gameData = new GameData(); // make class with default data
        tileInitializator.InitializeFirstTime(gameData);
        FileDataHandler.Write(gameData); // create json file and write default data

        SaveGame(); // save actual Unity data set in json file
    }

    [ContextMenu("Save")]
    public void SaveGame()
    {
        for (var i = 0; i < saveSystemDataObjectsList.Count; i++) saveSystemDataObjectsList[i].SaveData(gameData);

        FileDataHandler.Write(gameData);
    }

    public void LoadGame()
    {
        gameData = FileDataHandler.Read();

        if (gameData == null)
        {
            Debug.Log("No Save Files Found.");
            Debug.Log("Creating New Save File.");
            NewGame();
        }

        for (var i = 0; i < saveSystemDataObjectsList.Count; i++) saveSystemDataObjectsList[i].LoadData(gameData);
    }

    public void AddObjectToSaveSystemObjectsList(IDataPersistance saveSystemObject)
    {
        saveSystemDataObjectsList.Add(saveSystemObject);
    }


    private IEnumerator LoadStartDataCoroutine()
    {
        yield return new WaitForSeconds(loadStartDataDelay);
        LoadGame();
    }
}