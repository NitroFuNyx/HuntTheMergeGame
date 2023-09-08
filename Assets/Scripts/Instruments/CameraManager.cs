using Cinemachine;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;

    public void SetupNewCameraTarget(Transform cameraTarget)
    {
        cinemachineVirtualCamera.LookAt = cameraTarget;
    }
    public void SetupNewCameraFollow(Transform cameraFollow)
    {
        cinemachineVirtualCamera.Follow = cameraFollow;
    }
}
