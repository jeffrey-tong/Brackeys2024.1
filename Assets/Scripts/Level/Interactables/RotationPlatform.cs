using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class RotationPlatform : BaseInteractable
{
    [SerializeField] private RotationDirection rotationDirection = RotationDirection.CLOCKWISE;
    [SerializeField] private RotationMode rotationMode = RotationMode.PINGPONG;

    [SerializeField] private Transform pivotTransform;
    [SerializeField] private float rotationAngleBase;
    [SerializeField] private float rotationAngle;
    [SerializeField] private float rotationSpeed;

    private Vector2 pivotPoint;
    private bool isPingPongToEnd = false;

    private Coroutine rotateContinuousCoroutine;
    private bool isRotatingContinuous = false;

    private void Start()
    {
        pivotPoint = new Vector2(pivotTransform.position.x, pivotTransform.position.y);
        // Set the initial rotation to rotationAngleBase around the pivot point
        Vector3 pivotOffset = transform.position - (Vector3)pivotPoint;
        transform.position = (Vector3)pivotPoint + Quaternion.Euler(0f, 0f, rotationAngleBase) * pivotOffset;
        transform.rotation = Quaternion.Euler(0f, 0f, rotationAngleBase);
    }

    public override void Activate()
    {
        base.Activate();
        float targetRotation;
        float rotDirection = rotationDirection == RotationDirection.CLOCKWISE ? -1 : 1;
        if (canInteract)
        { 
            switch (rotationMode)
            {
                case RotationMode.PINGPONG:
                    canInteract = false;
                    targetRotation = isPingPongToEnd ? rotationAngleBase : rotationAngle;
                    StartCoroutine(RotatePingPong(targetRotation));
                    isPingPongToEnd = !isPingPongToEnd;
                    break;
                case RotationMode.INCREMENTAL:
                    canInteract = false;
                    targetRotation = rotationAngle;
                    rotDirection = rotationDirection == RotationDirection.CLOCKWISE ? -1 : 1;
                    StartCoroutine(RotateIncremental(targetRotation, rotDirection));
                    break;
                case RotationMode.CONTINUOUS:
                    if (isRotatingContinuous)
                    {
                        StopCoroutine(rotateContinuousCoroutine);
                    }
                    else
                    {
                        rotateContinuousCoroutine = StartCoroutine(RotateContinuous());
                    }
                    isRotatingContinuous = !isRotatingContinuous;
                    break;
                default:
                    break;
            }
        }
    }

    private IEnumerator RotatePingPong(float targetRotation)
    {
        float startRotation = transform.rotation.eulerAngles.z;

        float shortestPath = Mathf.DeltaAngle(startRotation, targetRotation);

        float step = 0f;
        float rotationDirection = Mathf.Sign(shortestPath);

        while (Mathf.Abs(step) < Mathf.Abs(shortestPath))
        {
            float rotationAmount = rotationSpeed * Time.deltaTime * rotationDirection;
            step += Mathf.Abs(rotationAmount);

            // Rotate around the pivot point
            transform.RotateAround((Vector3)pivotPoint, Vector3.forward, rotationAmount);

            yield return null;
        }
        canInteract = true;
    }

    private IEnumerator RotateIncremental(float targetRotation, float rotDirection)
    {
        float elapsedRotation = 0f;

        while (Mathf.Abs(elapsedRotation) < Mathf.Abs(targetRotation))
        {
            float rotationAmount = rotationSpeed * rotDirection * Time.deltaTime;

            // Rotate around the pivot point
            transform.RotateAround((Vector3)pivotPoint, Vector3.forward, rotationAmount);

            // Update elapsedRotation
            elapsedRotation += Mathf.Abs(rotationAmount);

            yield return null;
        }
        canInteract = true;
    }

    private IEnumerator RotateContinuous()
    {
        float rotDirection = rotationDirection == RotationDirection.CLOCKWISE ? -1 : 1;
        while (true)
        {
            // Rotate around the pivot point continuously
            transform.RotateAround((Vector3)pivotPoint, Vector3.forward, rotationSpeed * rotDirection * Time.deltaTime);

            yield return null;
        }
    }
}

public enum RotationMode
{
    PINGPONG,
    INCREMENTAL,
    CONTINUOUS
}

public enum RotationDirection
{
    CLOCKWISE,
    COUNTERCLOCKWISE
}
