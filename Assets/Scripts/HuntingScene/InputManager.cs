using System;
using System.Collections.Generic;
using UnityEngine;


public class InputManager : Singleton<InputManager>
{
    [Header("External References")]
    [Space] 
    [SerializeField] private Transform targetStartPosition;

    [SerializeField] private Transform target;
    [SerializeField] private LineRenderer line;
    [SerializeField] private JumpingController jumpingController;

    [SerializeField] private ParticleSystem playersAuraFX;
    [SerializeField] private ParticleSystem TargetFX;

    [Header("Move Data")]
    [Space] 
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField]private int numberOfObjects = 20;
    [SerializeField]private float arcHeight = 5.0f;
    
    private float horizontalMove = 0f;
    private float verticalMove = 0f;

    [SerializeField] private float radius = 15f;


    [Header("Joystick")] 
    [Space]
    [SerializeField]
    private Joystick joystick;

    public event Action OnPlayerStoppedTouch;


    private bool canMoveCamera = true;

    public bool CanMoveCamera
    {
        get => canMoveCamera;
        set => canMoveCamera = value;
    }

    [SerializeField] private List<Transform> objectsToPlace; // Массив объектов, которые нужно расставить по дуге

    public List<Transform> ObjectsToPlace
    {
        get => objectsToPlace;
        set => objectsToPlace = value;
    }


    private void Start()
    {
        line.positionCount = 20;
    }


    private void Update()
    {
        if (canMoveCamera)
        {
            MoveTarget();
            if (!jumpingController.IsJumpInProgress)
                PlaceObjectsAlongArc();
        }
    }


    public void SetNewParentForProjection(Transform parent)
    {
        targetStartPosition.parent = parent;
        target.parent = parent;
        targetStartPosition.position = target.position = parent.position;
    }

    private void MoveTarget()
    {
        horizontalMove = joystick.Horizontal * moveSpeed * Time.deltaTime;
        verticalMove = joystick.Vertical * moveSpeed * Time.deltaTime;


        Vector3 movement = Camera.main.transform.forward.normalized * verticalMove +
                           Camera.main.transform.right * horizontalMove;
        Vector3 newPosition = target.position + new Vector3(movement.x, 0, movement.z);

        Vector3 displacement = newPosition - targetStartPosition.position;
        float distance = displacement.magnitude;

        if (distance > radius)
        {
            Vector3 direction = displacement.normalized;
            newPosition = targetStartPosition.position + direction * radius;
        }

        float clampedX = Mathf.Clamp(newPosition.x, targetStartPosition.position.x - radius,
            targetStartPosition.position.x + radius);
        float clampedZ = Mathf.Clamp(newPosition.z, targetStartPosition.position.z - radius,
            targetStartPosition.position.z + radius);

        newPosition = new Vector3(clampedX, newPosition.y, clampedZ);

        target.position = newPosition;

        if (Input.GetMouseButtonDown(0) && !jumpingController.IsJumpInProgress)
        {
            playersAuraFX.Play();
            TargetFX.Play();
            line.enabled = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (!jumpingController.IsJumpInProgress)
                OnPlayerStoppedTouch?.Invoke();
            playersAuraFX.Stop();
            TargetFX.Stop();
            line.enabled = false;
        }
    }

    private void PlaceObjectsAlongArc()
    {
        Vector3 startPoint = targetStartPosition.position;
        Vector3 endPoint = target.position;
        float totalArcLength = Vector3.Distance(startPoint, endPoint);
        float startX = 0;
        for (int i = 0; i < numberOfObjects; i++)
        {
            float t = i / (float) (numberOfObjects - 1);
            float x = Mathf.Lerp(startX, totalArcLength, t);

            Vector3 position = Vector3.Lerp(startPoint, endPoint, t);

            position.y += arcHeight * 4 * t * (1 - t);

            if (i < ObjectsToPlace.Count)
            {
                ObjectsToPlace[i].position = position;
                line.SetPosition(i, ObjectsToPlace[i].position);
            }
            else
            {
                var newArcObject = Instantiate(ObjectsToPlace[0], position, Quaternion.identity);
                ObjectsToPlace.Add(newArcObject);
            }
        }
    }
}