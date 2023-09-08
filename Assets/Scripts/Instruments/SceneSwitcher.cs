using UnityEngine.SceneManagement;

public class SceneSwitcher : Singleton<SceneSwitcher>
{
    public void StartHunting()
    {
        SceneManager.LoadScene((int)SceneIds.Hunt);
    }
    public void StartMerging()
    {
        SceneManager.LoadScene((int)SceneIds.Merge);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}