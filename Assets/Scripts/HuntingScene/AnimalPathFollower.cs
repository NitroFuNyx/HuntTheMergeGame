using PathCreation;
using UnityEngine;

public class AnimalPathFollower:MonoBehaviour
{
    public Transform animal;
    //public PathCreator pathCreator;
    public EndOfPathInstruction endOfPathInstruction;
    public HuntParticipants participant;
    public float speed = 5f;
    public float distanceTravelled;
    public bool isActive;
    public bool isMainAnimal = false;
}