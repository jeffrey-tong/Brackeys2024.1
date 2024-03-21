using System.Collections;
using System.Collections.Generic;
using System.Drawing;
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

    private SpriteRenderer sr;
    private BoxCollider2D boxCollider;
    [SerializeField] private float platformLength;
    [SerializeField] private float platformWidth;

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

        sr = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();

        sr.size = new Vector2(platformLength, platformWidth);
        boxCollider.size = new Vector2(platformLength, platformWidth);

        if(rotationMode == RotationMode.CONTINUOUS)
        {
            rotateContinuousCoroutine = StartCoroutine(RotateContinuous());
            isRotatingContinuous = true;
        }
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

    private void OnDrawGizmos()
    {
        // Get the pivot point position
        Vector3 pivotPoint = pivotTransform.position;

        // Calculate the end points of the lines based on angles
        Vector3 endPoint1 = GetEndPoint(pivotPoint, rotationAngleBase);
        Vector3 endPoint2 = GetEndPoint(pivotPoint, rotationAngle);

        Gizmos.color = UnityEngine.Color.red;
        // Draw the lines
        Gizmos.DrawLine(pivotPoint, endPoint1);

        Gizmos.color = UnityEngine.Color.green;
        Gizmos.DrawLine(pivotPoint, endPoint2);

    }

    private void OnValidate()
    {
        sr = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        sr.size = new Vector2(platformLength, platformWidth);
        boxCollider.size = new Vector2(platformLength, platformWidth);
    }

    private Vector3 GetEndPoint(Vector3 pivot, float angle)
    {
        // Calculate the end point based on angle and distance (you can adjust the distance as needed)
        float distance = 5f;
        float radians = Mathf.Deg2Rad * angle;
        float x = pivot.x + distance * Mathf.Cos(radians);
        float y = pivot.y + distance * Mathf.Sin(radians);

        return new Vector3(x, y, pivot.z);
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
