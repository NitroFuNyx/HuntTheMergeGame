using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameLosePanel : MonoBehaviour
{
    [SerializeField] private HuntProccessor huntProccessor;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button backtomerge;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private int reward;
    [SerializeField] private CanvasGroup topUI;

    private InputManager inputManager;
    private ResourceManager resourceManager;
    private SceneSwitcher sceneSwitcher;

    private void Start()
    {
        huntProccessor = HuntProccessor.Instance;
        inputManager = InputManager.Instance;
        resourceManager = ResourceManager.Instance;
        sceneSwitcher = SceneSwitcher.Instance;
        huntProccessor.OnLostAllAnimals += ShowPanel;
        backtomerge.onClick.AddListener(BackToMerge);
        restartButton.onClick.AddListener(RestartGame);
    }

    private void OnDestroy()
    {
        huntProccessor.OnLostAllAnimals -= ShowPanel;
    }

    private void BackToMerge()
    {
        resourceManager.IncreaseMeatAmount(reward);
        sceneSwitcher.StartMerging();
    }

    private void RestartGame()
    {
        sceneSwitcher.RestartGame();
    }

    private void ShowPanel()
    {
        if (!EnvironmentManager.Instance.HasWon)
            canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
        inputManager.CanMoveCamera = false;
    }

    public void HidePanel()
    {
        topUI.alpha = 0;
        topUI.blocksRaycasts = false;
        topUI.interactable = false;
    }
}