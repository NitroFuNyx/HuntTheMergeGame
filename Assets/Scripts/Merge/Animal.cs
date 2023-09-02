using UnityEngine;

public class Animal : MonoBehaviour
{
    [SerializeField] private Outline outline;

    public void ActivateOutline(bool value)
    {
        outline.enabled = value;
    }
    
}
