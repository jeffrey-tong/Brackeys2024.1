using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TranslatingPlatform : BaseInteractable, IVelocity
{
    [Header("Components")]
    [SerializeField] private Transform startTransform;
    [SerializeField] private Transform endTransform;

    [SerializeField] private PlayMode mode = PlayMode.Single;

    [Header("Properties")]
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float duration = 0.5f;
    [SerializeField] private AnimationCurve translateCurve;

    private Vector2 startPoint;
    private Vector2 endPoint;

    private bool isMovingToEnd = true;

    public Vector2 Velocity => _velocity;

    public Vector2 _velocity;

    private void Awake()
    {
        startPoint = new Vector2(startTransform.position.x, startTransform.position.y);
        endPoint = new Vector2(endTransform.position.x, endTransform.position.y);

        transform.position = isMovingToEnd ? startPoint : endPoint;
    }

    private void Start()
    {
        if(mode == PlayMode.Single)
        {
            StartCoroutine(TranslateToPosition());
        }
    }

    public override void Activate()
    {
        base.Activate();
        if (canInteract)
        {
            canInteract = false;
            StartCoroutine(TranslateToPosition());
            isMovingToEnd = !isMovingToEnd;
        }
        
    }

    private IEnumerator TranslateToPosition()
    {
        Vector2 start = isMovingToEnd ? startPoint : endPoint;
        Vector2 end = isMovingToEnd ? endPoint : startPoint;

        float timeElapsed = 0.0f;

        while (timeElapsed <= duration)
        {
            timeElapsed += Time.fixedDeltaTime;
            float t = timeElapsed / duration;
            t = translateCurve.Evaluate(t);

            Vector2 translation = Vector2.LerpUnclamped(start, end, t);
            Vector2 deltaPosition = translation - (Vector2)transform.position;

            _velocity.x = deltaPosition.x / Time.fixedDeltaTime;
            _velocity.y = deltaPosition.y;

            transform.position = translation;

            yield return new WaitForFixedUpdate();
        }

        _velocity = Vector2.zero;
        canInteract = true;

        if(mode == PlayMode.Single)
        {
            isMovingToEnd = !isMovingToEnd;
            StartCoroutine(TranslateToPosition());
        }
    }

    private void OnDrawGizmos()
    {
        // Store the current matrix
        Matrix4x4 originalMatrix = Gizmos.matrix;

        // Apply the object's rotation to the Gizmos.matrix
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);

        // Draw the rotated wire cube
        Gizmos.DrawWireCube(endTransform.position - transform.position, new Vector3(transform.localScale.x, transform.localScale.y, 0));

        // Restore the original matrix
        Gizmos.matrix = originalMatrix;
    }
}

public interface IVelocity
{
    public Vector2 Velocity { get; }
}

public enum PlayMode
{
    Single,
    Pingpong,
}
