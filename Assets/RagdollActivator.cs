using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollActivator : MonoBehaviour
{
    [SerializeField] private List<Rigidbody> rigidbodies;
    [SerializeField] private Animator animator;

    private void Start()
    {
        DeactivateRagdoll();
    }

    [ContextMenu("Activate")]
    public void ActivateRagdoll( )
    {
        foreach (var rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = false;
            rigidbody.useGravity = true;
            animator.enabled = false;
        }
    }
    [ContextMenu("Deactivate")]

    public void DeactivateRagdoll()
    {
        foreach (var rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = true;
            rigidbody.useGravity = false;
            animator.enabled = true;

        }
    }
}
