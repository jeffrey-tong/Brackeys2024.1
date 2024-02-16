using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Magnet : BaseInteractable
{
    private SpriteRenderer sr;
    [SerializeField] private BoxCollider2D magnetBoxForceCollider;
    [SerializeField] private CircleCollider2D magnetCircleForceCollider;
    private PointEffector2D pointEffector;

    [SerializeField] private MagnetMode magnetMode = MagnetMode.ATTRACT;
    [SerializeField] private MagnetDirection magnetDirection = MagnetDirection.NORTH;
    [SerializeField] private float magnetForce;

    //BoxCollider size and offset
    [SerializeField] private Vector2 magnetBoxForceColliderOffset;
    [SerializeField] private float magnetLength;
    [SerializeField] private float magnetWidth;
    //Circle collider radius
    [SerializeField] private float magnetRadius;

    private Collider2D magnetCollider;
    [SerializeField] LayerMask pushableLayerMask;
    [SerializeField] private bool isMagnetOn = false;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        pointEffector = GetComponent<PointEffector2D>();

        SetupMagnet();
    }

    public override void Activate()
    {
        isMagnetOn = !isMagnetOn;
        if (isMagnetOn)
        {
            sr.color = Color.green;
            pointEffector.enabled = true;
        }
        else
        {
            sr.color = Color.red;
            pointEffector.enabled = false;
        }
        //Sets if metal boxes can be moved or not
        //Check for radial mode
        if (magnetCircleForceCollider.enabled)
        {
            Collider2D[] circleColliders = Physics2D.OverlapCircleAll(transform.position, magnetRadius, pushableLayerMask);
            foreach (Collider2D collider in circleColliders)
            {
                MoveableObject pushObject = collider.GetComponent<MoveableObject>();
                if (pushObject != null)
                {
                    pushObject.SetPushable(isMagnetOn, Vector2.zero);
                }
            }
        }
        if (magnetBoxForceCollider.enabled)
        {
            // Get all colliders within the box collider with offset
            Collider2D[] boxColliders = Physics2D.OverlapBoxAll((Vector2)transform.position + magnetBoxForceCollider.offset, new Vector2(magnetLength, magnetWidth), 0f, pushableLayerMask);

            // Process the results for the box collider
            foreach (Collider2D collider in boxColliders)
            {
                MoveableObject pushObject = collider.GetComponent<MoveableObject>();
                if (pushObject != null)
                {
                    pushObject.SetPushable(isMagnetOn, Vector2.zero);
                }
            }
        }
    }
    private void SetupMagnet()
    {
        magnetBoxForceCollider.offset = magnetBoxForceColliderOffset;
        magnetBoxForceCollider.size = new Vector2(magnetLength, magnetWidth);

        magnetCircleForceCollider.radius = magnetRadius;

        if (magnetMode == MagnetMode.ATTRACT) pointEffector.forceMagnitude = -magnetForce;
        else pointEffector.forceMagnitude = magnetForce;

        if (magnetDirection == MagnetDirection.RADIAL)
        {
            magnetBoxForceCollider.enabled = false;
            magnetCircleForceCollider.enabled = true;
        }
        else
        {
            magnetBoxForceCollider.enabled = true;
            magnetCircleForceCollider.enabled = false;
        }

        if (!isMagnetOn) pointEffector.enabled = false;
        else pointEffector.enabled = true;
    }

    private void OnValidate()
    {
        sr = GetComponent<SpriteRenderer>();
        pointEffector = GetComponent<PointEffector2D>();
        SetupMagnet();

        transform.rotation = Quaternion.Euler(0f, 0f, (int)magnetDirection * 90f);
        EditorUtility.SetDirty(this);
    }

    private void OnDrawGizmos()
    {
        // Store the current matrix
        Matrix4x4 originalMatrix = Gizmos.matrix;

        // Apply the object's rotation to the Gizmos.matrix
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);

        // Draw the rotated wire cube
        Gizmos.DrawWireCube(magnetBoxForceColliderOffset, new Vector3(magnetLength, magnetWidth, 0));

        // Restore the original matrix
        Gizmos.matrix = originalMatrix;

        Gizmos.DrawWireSphere(transform.position, magnetRadius);
    }
}

public enum MagnetMode
{
    ATTRACT,
    REPEL
}

public enum MagnetDirection
{
    EAST,
    NORTH,
    WEST,
    SOUTH,
    RADIAL
}

