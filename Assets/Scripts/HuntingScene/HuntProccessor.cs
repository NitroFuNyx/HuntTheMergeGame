using System;
using System.Collections.Generic;
using System.Linq;
using PathCreation;
using UnityEngine;

public class HuntProccessor : Singleton<HuntProccessor>
{
    [SerializeField] private List<AnimalPathFollower> animalPathFollowers;
    [SerializeField] private DeerCollisionManager deerTarget;
    [SerializeField] private bool isHuntStarted;
    [SerializeField] private AnimalPathFollower currentDeer;
    [SerializeField] private AnimalPathFollower currentHunter;
    [SerializeField] private AnimalPathFollower nextHunter;
    [SerializeField] private PathCreator path;

    [SerializeField] private float speedForAllAnimals = 5f;
     private InputManager inputManager;
     private JumpingController jumpingController;
     private CameraManager cameraManager;

     private bool canLoose;
     public event Action OnLostAllAnimals; 
    void Start()
    {
        inputManager = InputManager.Instance;
        jumpingController = JumpingController.Instance;
        jumpingController.OnCurrentJumperDied += SwitchActiveAnimal;
        cameraManager = CameraManager.Instance;
       
        InitializeNewDeer();
    }

    private void OnDestroy()
    {
        jumpingController.OnCurrentJumperDied -= SwitchActiveAnimal;

    }

    public void InitializeNewDeer()
    {
        currentDeer = animalPathFollowers.FirstOrDefault(deer => deer.participant == HuntParticipants.Deer);
        if (currentDeer == null) throw new Exception("List of followers doesn't have deers!");
        currentDeer.isActive = true;
        isHuntStarted = true;
    }

    public void InitializingHunter()
    {
        nextHunter = animalPathFollowers.FirstOrDefault(deer => deer.participant == HuntParticipants.Hunter&&deer!=currentHunter);

        if (nextHunter == null) return;
            
        nextHunter.isActive = true;
        nextHunter.distanceTravelled = currentDeer.distanceTravelled - 15f;
    }

    private void SwitchActiveAnimal()
    {
        if (nextHunter == null && canLoose) OnLostAllAnimals?.Invoke();
        else if (nextHunter == null) return;
            currentHunter = nextHunter;
        currentHunter.speed = 8;
        cameraManager.SetupNewCameraFollow(currentHunter.animal);
        jumpingController.SetupNewAnimal(currentHunter.animal);
        inputManager.SetNewParentForProjection(currentHunter.transform);
        InitializingHunter();
    }
    public void ExcludeFromHunt()
    {
        animalPathFollowers.Remove(currentHunter);
    }

    public void AddToHunt(PoolItem animal)
    {
        if (animal.TryGetComponent(out AnimalPathFollower follower))
        {
            animalPathFollowers.Add(follower);
            follower.speed = speedForAllAnimals;
            InitializingHunter();
            var iterator = 0;
            foreach (var animalHunters in animalPathFollowers)
            {
                if (animalHunters.participant == HuntParticipants.Hunter)
                {
                    iterator++;
                }
            }

            if (iterator == 1)
            {
                SwitchActiveAnimal();
                canLoose = true;
            }
        }

    }
    void Update()
    { 
        if (!isHuntStarted) return;
        foreach (var follower in animalPathFollowers)
        {
            if (!follower.isActive) continue;
            if (path != null)
            {
                if (follower.speed > speedForAllAnimals&& currentDeer.distanceTravelled-follower.distanceTravelled<5f)
                {
                    follower.speed = speedForAllAnimals;
                }
             follower.distanceTravelled +=follower.speed * Time.deltaTime;
             follower.transform.position = path.path.GetPointAtDistance(follower.distanceTravelled, follower.endOfPathInstruction);
             follower.transform.rotation = path.path.GetRotationAtDistance(follower.distanceTravelled, follower.endOfPathInstruction);
             follower.transform.eulerAngles += new Vector3(0, 0, 90f);
            }
        }

    }

   



    
}