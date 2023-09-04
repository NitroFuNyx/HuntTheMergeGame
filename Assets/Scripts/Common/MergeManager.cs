using UnityEngine;

public class MergeManager : MonoBehaviour
{
    [SerializeField] private AnimalSpawner animalSpawner;
    [SerializeField] private ParticleSystem upgradeFX;

    public bool MergeAnimals(TileHolder tile, Animal animal1, Animal animal2)
    {
        if (animal1.AnimalData.level >= 3 || animal2.AnimalData.level >= 3) return true;

        if (animal1.AnimalData.level != animal2.AnimalData.level) return true;
        upgradeFX.transform.position = animal2.transform.position;

        upgradeFX.Stop();
        upgradeFX.Play();
        animalSpawner.ReturnAnimalToPool(animal1.ItemPool);
        animalSpawner.ReturnAnimalToPool(animal2.ItemPool);

        animalSpawner.SpawnAnimal((PoolItemsTypes) ((int) animal1.ItemPool.PoolItemType + 1), tile);
        tile.OccupyTile(true, (int) animal1.ItemPool.PoolItemType + 1);
        return false;
    }
}