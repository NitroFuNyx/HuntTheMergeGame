using UnityEngine;

public class Animal : MonoBehaviour
{
    [SerializeField] private AnimalsData animalData;
    [SerializeField] private PoolItem poolItem;
    [SerializeField] private Outline outline;
    [SerializeField] private BoxCollider boxCollider;

    public AnimalsData AnimalData => animalData;

    public PoolItem ItemPool => poolItem;

    public void ActivateOutline(bool value)
    {
        outline.enabled = value;
    }

    public void ActivateCollider(bool value)
    {
        boxCollider.enabled = value;
    }
    
}
