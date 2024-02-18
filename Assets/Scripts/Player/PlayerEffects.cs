using UnityEngine;

public class PlayerEffects : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private PlayerLocomotion m_Locomotion;



    [Header("Visual Effects")]
    [SerializeField] private ParticleSystem followVFX;
    [SerializeField] private ParticleSystem landedVFX;

    [Header("Sound Effects")]
    [SerializeField] private AudioClip landedSFX;
    [SerializeField] private AudioClip jumpedSFX;

    private ParticleSystem followVFXInstance;

    private void Awake()
    {
        if (m_Locomotion == null)
            m_Locomotion = GetComponent<PlayerLocomotion>();
    }

    private void Start()
    {
        m_Locomotion.OnPlayerLanded += OnPlayerLanded_Callback;
        m_Locomotion.OnPlayerJumped += OnPlayerJumped_Callback;

        //followVFXInstance = Instantiate(followVFX, transform.position, Quaternion.identity);
    }

    //private void Update()
    //{
    //    if (m_Locomotion.IsPlayerGrounded())
    //    {
    //        float dir = Mathf.Sign(m_Locomotion.GetXInput());
    //        Vector3 rotation = dir >= 1 ? Vector3.up * 180 : Vector3.zero;

    //        followVFXInstance.transform.position = transform.position + (Vector3.right * -dir * 2);
    //        followVFXInstance.transform.eulerAngles = rotation;
    //    }
    //}

    private void OnPlayerJumped_Callback()
    {
        if (jumpedSFX != null)
            AudioManager.Instance.PlayAudioSFX(jumpedSFX);
    }

    private void OnPlayerLanded_Callback()
    {
        if (landedSFX != null)
            AudioManager.Instance.PlayAudioSFX(landedSFX);

        if (landedVFX != null)
            Instantiate(landedVFX, transform.position, Quaternion.identity);
    }

    //private void OnDestroy()
    //{
    //    if (followVFXInstance != null)
    //    {
    //        Destroy(followVFXInstance.gameObject);
    //    }
    //}
}