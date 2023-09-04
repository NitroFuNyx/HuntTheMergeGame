using System;
using System.Collections.Generic;

[Serializable]
public class GameData
{
    public Secureint meatAmount;
    public List<TileHolder.ViewModel> tilesList;

    public GameData()
    {
        meatAmount = new Secureint();
        tilesList = new List<TileHolder.ViewModel>();
        //  tilesList.ForEach(tile => tile.IsLocked = true);
    }
}