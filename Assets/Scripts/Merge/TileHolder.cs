using System;
using UnityEngine;

public class TileHolder : MonoBehaviour
{
    [SerializeField] private GameObject blockedScreen;
    [SerializeField] private ViewModel mViewModel;

    public ViewModel MViewModel
    {
        get => mViewModel;

        set
        {
            blockedScreen.SetActive(value.IsLocked);
            mViewModel = value;
        }
    }

    public void BlockTile()
    {
        mViewModel.IsLocked = true;
        blockedScreen.SetActive(true);
    }

    public void UnBlockTile()
    {
        mViewModel.IsLocked = false;
        blockedScreen.SetActive(false);
    }

    public void OccupyTile(bool isOccupied, int level)
    {
        mViewModel.IsOccupied = isOccupied;
        mViewModel.Level = level;
    }

    [Serializable]
    public class ViewModel
    {
        public int TileId;
        public bool IsOccupied;
        public int Level;
        public bool IsLocked = true;
    }
}