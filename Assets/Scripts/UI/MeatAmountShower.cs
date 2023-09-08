using TMPro;
using UnityEngine;


public class MeatAmountShower : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI meatText;
    private void Start()
    {
        ResourceManager.Instance.OnMeatAmountChanged += ChangeAmountOfMeat;
    }

    private void OnDestroy()
    {
        ResourceManager.Instance.OnMeatAmountChanged -= ChangeAmountOfMeat;

    }

    private void ChangeAmountOfMeat(int value)
    {
        meatText.text = value.ToString();
    }
}
