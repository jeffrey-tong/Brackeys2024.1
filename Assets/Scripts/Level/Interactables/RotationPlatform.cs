using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.UIElements;

public enum RotationMode
{
    TOGGLE,
    CONTINUOUS
}

public enum RotationDirection
{
    CLOCKWISE,
    COUNTERCLOCKWISE
}

public class RotationPlatform : BaseInteractable
{
    [SerializeField] private RotationDirection rotationDirection = RotationDirection.CLOCKWISE;
    [SerializeField] private RotationMode rotationMode = RotationMode.TOGGLE;

    [SerializeField] private Transform pivotTransform;
    [SerializeField] private float rotationAngleBase;
    [SerializeField] private float rotationAngle;
    [SerializeField] private float rotationSpeed;

    private Vector2 pivotPoint;
    private bool isRotatingToEnd = false;
    private bool isRotating = false;

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
        if (!isRotating)
        {
            isRotating = true;
            switch (rotationMode)
            {
                case RotationMode.TOGGLE:
                    targetRotation = isRotatingToEnd ? rotationAngleBase : rotationAngle;
                    StartCoroutine(RotateToggle(targetRotation));
                    isRotatingToEnd = !isRotatingToEnd;
                    break;
                case RotationMode.CONTINUOUS:
                    targetRotation = rotationAngle;
                    float rotDirection = rotationDirection == RotationDirection.CLOCKWISE ? -1 : 1;
                    StartCoroutine(RotateContinuous(targetRotation, rotDirection));
                    break;
                default:
                    break;
            }
        }
    }

    private IEnumerator RotateToggle(float targetRotation)
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
        isRotating = false;
    }

    private IEnumerator RotateContinuous(float targetRotation, float rotDirection)
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
        isRotating = false;
    }
}
