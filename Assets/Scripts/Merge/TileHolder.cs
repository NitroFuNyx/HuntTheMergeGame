using System;
using UnityEngine;

public class TileHolder : MonoBehaviour
{

    [SerializeField] private GameObject blockedScreen;
    
    
    
    [SerializeField] private ViewModel _mViewModel;

    public ViewModel MViewModel
    {
        get => _mViewModel;

        set
        {
            blockedScreen.SetActive(value.IsLocked); 
            _mViewModel = value;
        }
    }

    public void BlockTile()
    {
        _mViewModel.IsLocked = true;
        blockedScreen.SetActive(true);
    }
    public void UnBlockTile()
    {
        _mViewModel.IsLocked = false;
      blockedScreen.SetActive(false);
    }

    public void OccupyTile(bool isOccupied,int level)
    {
        _mViewModel.IsOccupied = isOccupied;
        _mViewModel.Level = level;

    }
    [Serializable]
    public class ViewModel
    {
        public int TileId;
        public bool IsOccupied;
        public int Level;
        public bool IsLocked=true;
    }
}
