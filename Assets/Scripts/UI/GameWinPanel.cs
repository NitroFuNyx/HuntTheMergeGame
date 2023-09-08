using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameWinPanel : MonoBehaviour
{
   
   [SerializeField] private HealthBar healthBar;
   [SerializeField] private Button restartButton;
   [SerializeField] private Button backtomerge;
   [SerializeField] private CanvasGroup canvasGroup;
   [SerializeField] private TextMeshProUGUI killsText;
   [SerializeField] private int reward;
   [SerializeField] private CanvasGroup topUI;

   
   private InputManager inputManager;
   private ResourceManager resourceManager;
   private SceneSwitcher sceneSwitcher;
   private void Start()
   {
      inputManager = InputManager.Instance;
      resourceManager = ResourceManager.Instance;
      sceneSwitcher = SceneSwitcher.Instance;
      healthBar.OnDeerKilled += ShowPanel;
      backtomerge.onClick.AddListener(BackToMerge);
      restartButton.onClick.AddListener(RestartGame);
   }
   private void OnDestroy()
   {
      healthBar.OnDeerKilled -= ShowPanel;

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
      killsText.text = "1/1";
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