using System;
using UnityEngine;

public class Animal : MonoBehaviour
{
    [SerializeField] private AnimalsData animalData;
    [SerializeField] private PoolItem poolItem;
    [SerializeField] private Outline outline;
    [SerializeField] private BoxCollider boxCollider;
    [SerializeField] private Rigidbody headRigidBody;
    [SerializeField] private RagdollActivator ragdollActivator;
    [SerializeField] private AnimalPathFollower animalPathFollower;
    public AnimalsData AnimalData => animalData;

    public PoolItem ItemPool => poolItem;

    public Rigidbody HeadRigidBody => headRigidBody;

    public RagdollActivator RagdollActivator => ragdollActivator;

    public AnimalPathFollower PathFollower => animalPathFollower;

    public void ActivateOutline(bool value)
    {
        outline.enabled = value;
    }

    public void ActivateCollider(bool value)
    {
        boxCollider.enabled = value;
    }

   

   
}
