using System;
using UnityEngine;
using DG.Tweening;

public class JumpingController : Singleton<JumpingController>
{
    [SerializeField] private Transform animal;

    [SerializeField] private Animator animator;
    [SerializeField] private float jumpTime;
    [SerializeField] private float timeBetweenTargets;

    private int targetsCounter;

    [SerializeField] private bool isJumpInProgress;

    public bool IsJumpInProgress => isJumpInProgress;
    private InputManager inputManager;
    private HuntProccessor huntProccessor;


    public event Action OnCurrentJumperDied;

    private void Start()
    {
        inputManager = InputManager.Instance;
        inputManager.OnPlayerStoppedTouch += StartJump;
        huntProccessor = HuntProccessor.Instance;
    }


    private void OnDestroy()
    {
        inputManager.OnPlayerStoppedTouch -= StartJump;
    }

    private void StartJump()
    {
        targetsCounter = 0;
        animator.SetTrigger("Jump");
        isJumpInProgress = true;
        huntProccessor.ExcludeFromHunt();
        LookAtJumpDirection();
        Jump();
    }

    private void Jump()
    {
        if (targetsCounter >= inputManager.ObjectsToPlace.Count)
        {
            animal.TryGetComponent(out RagdollActivator ragdollActivator);
            ragdollActivator.ActivateRagdoll();
            isJumpInProgress = false;
            OnCurrentJumperDied?.Invoke();
            return;
        }

        timeBetweenTargets = jumpTime / inputManager.ObjectsToPlace.Count;
        animal.DOMove(inputManager.ObjectsToPlace[targetsCounter].position, timeBetweenTargets).SetEase(Ease.Linear)
            .OnComplete(
                () =>
                {
                    targetsCounter++;
                    Jump();
                });
    }

    private void LookAtJumpDirection()
    {
        animal.DOLookAt(inputManager.ObjectsToPlace[inputManager.ObjectsToPlace.Count - 1].position, .7f);
    }

    public void SetupNewAnimal(Transform newAnimal)
    {
        animal = newAnimal;
        animator = animal.GetComponent<Animator>();
    }
}