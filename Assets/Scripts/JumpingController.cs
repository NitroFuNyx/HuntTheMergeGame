using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class JumpingController : MonoBehaviour
{
    [SerializeField] private Transform animal;

    [SerializeField] private Transform target;
    public List<Transform> objectsToPlace; // Массив объектов, которые нужно расставить по дуге
    public int numberOfObjects = 10; // Количество объектов для размещения
    public float arcHeight = 5.0f; // Высота дуги
    public float horizontalSpacing = 1.0f; // Горизонтальное расстояние между объектами
    [SerializeField] private Animator animator;

    [SerializeField] private List<Transform> targets;
    [SerializeField] private int targetsCounter;
    [SerializeField] private float jumpTime;
    [SerializeField] private float Time;

    [SerializeField] private LineRenderer line;
    [SerializeField] private bool isJumpInProgress;
    private void Update()
    {
        if (!isJumpInProgress) 
         PlaceObjectsAlongArc();

        if (Input.GetMouseButtonDown(0))
        {
            targetsCounter = 0;
            animator.SetTrigger("Jump");
            isJumpInProgress = true;
            Jump();
        }
    }

    private void Start()
    {
        line.positionCount = 20;

    }

    private void Jump()
    {

        if (targetsCounter >= objectsToPlace.Count)
        {
            animator.SetTrigger("Idle");
            return;
        }
        Time =  jumpTime / objectsToPlace.Count;
        Debug.Log(targetsCounter);
            animal.DOMove(objectsToPlace[targetsCounter].position, Time).SetEase(Ease.Linear).OnComplete(
                ()=>
                {
                    targetsCounter++;
                    Jump();
                });

    }
    
    private void PlaceObjectsAlongArc()
    {
        Vector3 startPoint = animal.position;
        Vector3 endPoint = target.position;
        float totalArcLength = Vector3.Distance(startPoint, endPoint);
        float startX = 0;
        for (int i = 0; i < numberOfObjects; i++)
        {
            float t = i / (float)(numberOfObjects - 1); // Нормализованный параметр времени от 0 до 1
            float x = Mathf.Lerp(startX, totalArcLength, t); // Горизонтальная позиция объекта

            Vector3 position = Vector3.Lerp(startPoint, endPoint, t); // Интерполяция позиции

            position.y += arcHeight * 4 * t * (1 - t); // Вертикальная позиция объекта (уравнение параболы)

            if (i < objectsToPlace.Count)
            {
                objectsToPlace[i].position = position;
                line.SetPosition(i,objectsToPlace[i].position);
            }
            else
            {
                // Если массив объектов для размещения меньше, чем numberOfObjects,
                // создайте новые объекты, чтобы заполнить дугу.
                var newArcObject =Instantiate(objectsToPlace[0], position, Quaternion.identity);
                objectsToPlace.Add(newArcObject);
            }
        }
    }
}
