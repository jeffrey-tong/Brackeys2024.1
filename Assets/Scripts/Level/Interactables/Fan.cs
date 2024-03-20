using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class Fan : BaseInteractable
{ 
    private SpriteRenderer sr;
    private BoxCollider2D boxCollider;
    private AreaEffector2D areaEffector;
    [SerializeField] private FanDirection fanDirection = FanDirection.EAST;
    
    //BoxCollider size and offset
    [SerializeField] private Vector2 offset;
    [SerializeField] private float fanLength;
    [SerializeField] private float fanWidth;

    //Area effector direction and force
    [SerializeField] private FanMode fanMode = FanMode.PUSH;
    [SerializeField] private float fanForce;
    [SerializeField] private bool isFanOn = false;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        areaEffector = GetComponent<AreaEffector2D>();

        sr.color = Color.red;

        if (isFanOn)
        {
            sr.color = Color.green;
            areaEffector.enabled = true;
        }
        else
        {
            sr.color = Color.red;
            areaEffector.enabled = false;
        }
    }

    public override void Activate()
    {
        isFanOn = !isFanOn;

        if (isFanOn)
        {
            sr.color = Color.green;
            areaEffector.enabled = true;
        }
        else
        {
            sr.color = Color.red;
            areaEffector.enabled = false;
        }
    }

    private void SetupFan()
    {
        boxCollider.offset = offset;
        boxCollider.size = new Vector2(fanLength, fanWidth);

        areaEffector.forceMagnitude = fanForce;
        // Calculate the base angle based on fanMode
        float baseAngle = fanMode == FanMode.PUSH ? 0f : 180f;

        // Calculate the total angle including the object's rotation
        float totalAngle = baseAngle + ((int)fanDirection * 90f);

        // Ensure the angle stays within 0-360 range
        totalAngle %= 360f;
        if (totalAngle < 0)
            totalAngle += 360f;

        // Set the force angle
        areaEffector.forceAngle = totalAngle;

        transform.rotation = Quaternion.Euler(0f, 0f, (int)fanDirection * 90f);

        if (!isFanOn) areaEffector.enabled = false;
        else areaEffector.enabled = true;
    }

    private void OnValidate()
    {
        sr = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        areaEffector = GetComponent<AreaEffector2D>();
        SetupFan();

        transform.rotation = Quaternion.Euler(0f, 0f, (int)fanDirection * 90f);
        //EditorUtility.SetDirty(this);
    }

    private void OnDrawGizmos()
    {
        // Store the current matrix
        Matrix4x4 originalMatrix = Gizmos.matrix;

        // Apply the object's rotation to the Gizmos.matrix
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);

        // Draw the rotated wire cube
        Gizmos.DrawWireCube(offset, new Vector3(fanLength, fanWidth, 0));

        // Restore the original matrix
        Gizmos.matrix = originalMatrix;
    }
}

public enum FanMode
{
    PUSH,
    PULL
}

public enum FanDirection
{
    EAST,
    NORTH,
    WEST,
    SOUTH
}
