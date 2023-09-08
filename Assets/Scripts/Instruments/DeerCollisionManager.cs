using System;
using UnityEngine;

public class DeerCollisionManager : MonoBehaviour
{
    [SerializeField] private CharacterJoint characterJoint;
    [SerializeField] private HealthBar healthBar;

    private void Start()
    {
        healthBar.SetMaxHealth(30);
    }
    public void SetJoint(Rigidbody rigidbody)
    {
        characterJoint.connectedBody = rigidbody;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Hunter")) return;
        if (other.TryGetComponent(out Animal animal))
        {   
            healthBar.SetHealth(animal.AnimalData.attackStat);
            SetJoint(animal.HeadRigidBody);
            animal.RagdollActivator.ActivateRagdoll();
        }
    }
}
