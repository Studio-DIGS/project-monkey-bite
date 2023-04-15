using UnityEngine;

public class SimplePathCamera : MonoBehaviour
{
    private Transform cameraContainer;
    private Transform targetPosition;
    private PathTransform pathTransform;

    public void Initialize(Transform cameraContainer, PathTransform pathTransform, Transform targetPosition)
    {
        this.cameraContainer = cameraContainer;
        this.pathTransform = pathTransform;
        this.targetPosition = targetPosition;
    }

    public void UpdateCamera()
    {
        cameraContainer.position = targetPosition.position;
        cameraContainer.rotation = Quaternion.LookRotation(pathTransform.WCurrentNormal, pathTransform.WCurrentUp);
    }
}
