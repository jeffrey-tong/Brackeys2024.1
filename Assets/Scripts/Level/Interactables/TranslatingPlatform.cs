using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslatingPlatform : BaseInteractable
{ 
    [SerializeField] private Transform startTransform;
    [SerializeField] private Transform endTransform;
    [SerializeField] private float translateSpeed;

    private Vector2 startPoint;
    private Vector2 endPoint;

    private bool isMovingToEnd = false;
    private bool isMoving = false;

    private void Start()
    {
        startPoint = new Vector2(startTransform.position.x, startTransform.position.y);
        endPoint = new Vector2(endTransform.position.x, endTransform.position.y);
    }

    public override void Activate()
    {
        base.Activate();
        if (!isMoving)
        {
            isMoving = true;
            Vector3 targetPosition = isMovingToEnd ? startPoint : endPoint;
            StartCoroutine(TranslateToPosition(targetPosition));
            isMovingToEnd = !isMovingToEnd;
        }
        
    }

    private IEnumerator TranslateToPosition(Vector3 targetPosition)
    {
        while (Vector2.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, translateSpeed * Time.deltaTime);

            yield return null;
        }
        isMoving = false;
    }
}
