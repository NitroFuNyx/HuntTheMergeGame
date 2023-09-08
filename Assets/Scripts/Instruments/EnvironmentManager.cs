
using UnityEngine;

public class EnvironmentManager : Singleton<EnvironmentManager>
{
    [SerializeField] private Gamemodes gamemode;
    [SerializeField] private bool hasWon;


    public Gamemodes Gamemode => gamemode;

    public bool HasWon => hasWon;
}
