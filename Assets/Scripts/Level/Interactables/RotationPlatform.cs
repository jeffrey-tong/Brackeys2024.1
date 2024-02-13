using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private bool isRotatingToEnd = true;
    private float remainingRotation;

    private void Start()
    {
        pivotPoint = new Vector2(pivotTransform.position.x, pivotTransform.position.y);
    }

    public override void Activate()
    {
        base.Activate();

        switch (rotationMode)
        {
            case RotationMode.TOGGLE:
                remainingRotation = isRotatingToEnd ? rotationAngle : rotationAngleBase;
                break;
            case RotationMode.CONTINUOUS:
                remainingRotation = rotationAngle;
                RotateContinuous();
                break;
            default:
                break;
        }
    }

    private void RotateToggle()
    {
        while (remainingRotation > 0f)
        {
            float rotationAmount = (rotationDirection == RotationDirection.CLOCKWISE ? 1 : -1) * rotationSpeed * Time.deltaTime;
            rotationAmount = Mathf.Clamp(rotationAmount, -remainingRotation, remainingRotation);

            // Rotate around the pivot point
            transform.RotateAround((Vector3)pivotPoint, Vector3.forward, rotationAmount);

            // Recalculate the offset after rotation
            Vector2 offset = (Vector2)pivotPoint - (Vector2)transform.position;

            // Update remainingRotation
            remainingRotation -= Mathf.Abs(rotationAmount);
        }
    }

    private void RotateContinuous()
    {
        while (remainingRotation > 0f)
        {
            float rotationAmount = (rotationDirection == RotationDirection.CLOCKWISE ? 1 : -1) * rotationSpeed * Time.deltaTime;
            rotationAmount = Mathf.Clamp(rotationAmount, -remainingRotation, remainingRotation);

            // Rotate around the pivot point
            transform.RotateAround((Vector3)pivotPoint, Vector3.forward, rotationAmount);

            // Recalculate the offset after rotation
            Vector2 offset = (Vector2)pivotPoint - (Vector2)transform.position;

            // Update remainingRotation
            remainingRotation -= Mathf.Abs(rotationAmount);
        }
    }
}
