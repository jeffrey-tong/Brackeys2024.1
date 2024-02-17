using UnityEngine;

public class PlayerCameraManager : MonoBehaviour
{
    [Header("Component")]
    [SerializeField] private PlayerController m_FollowPlayer;

    [Header("Properties")]
    [SerializeField] private Vector2 offset;
    [SerializeField] private Vector2 deadZone;
    [SerializeField] private Vector2 smoothTime;
    private Vector2 velocity;
    private Vector2 target;

    private void Start()
    {
        m_FollowPlayer = PlayerController.Current;

        PlayerController.OnPlayerChanged += Player_OnChangedCallback;
    }

    private void LateUpdate()
    {
        if (m_FollowPlayer == null) return;

        float currentX = transform.position.x;
        float currentY = transform.position.y;

        float desiredX = m_FollowPlayer.transform.position.x;
        float desiredY = m_FollowPlayer.transform.position.y + offset.y;

        float diffX = Mathf.Abs(desiredX - currentX);
        bool updateX = diffX >= deadZone.x;
        Debug.Log($"Current diff is: {diffX}");
        if (updateX)
        {
            target.x = desiredX;
            //bool isPlayerMoving = !Mathf.Approximately(m_FollowPlayer.GetLocomotion().GetXInput(), 0);
            //float lookAhead = isPlayerMoving ? offset.x * m_FollowPlayer.GetLocomotion().GetXInput() : 0;

            //target.x += lookAhead;
        }

        bool updateY = Mathf.Abs(desiredY-currentY) > deadZone.y;
        if (updateY) 
        {
            target.y = desiredY;
        }

        float translationX = Mathf.SmoothDamp(currentX, target.x, ref velocity.x, smoothTime.x);
        float translationY = Mathf.SmoothDamp(currentY, target.y, ref velocity.y, smoothTime.y);

        transform.position = new Vector3(translationX, translationY, -10);
    }

    private void Player_OnChangedCallback()
    {
        m_FollowPlayer = PlayerController.Current;
    }

    private void OnDrawGizmosSelected()
    {
        if (m_FollowPlayer == null) return;

        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Vector3 position = transform.position;
        position.z = m_FollowPlayer.transform.position.z;
        Gizmos.DrawCube(position, new Vector3(deadZone.x * 2, deadZone.y * 2, 0));
    }

    private void OnValidate()
    {
        if (m_FollowPlayer == null) return;

        float targetX = m_FollowPlayer.transform.position.x;
        float targetY = m_FollowPlayer.transform.position.y + offset.y;

        transform.position = new Vector3(targetX, targetY, -10);
    }
}
