using UnityEngine;

public class RecallAbility : PlayerAbility
{
    [SerializeField] private GameObject recallObject;
    [SerializeField] private AudioClip SpawnSFX;
    [SerializeField] private AudioClip teleportSFX;

    private GameObject recallInstance;

    public override bool CanPerformAbility()
    {
        return true;
    }

    public override void PerformAbility()
    {
        if (recallInstance != null)
        {
            transform.position = recallInstance.transform.position;
            AudioManager.Instance.PlayAudioSFX(teleportSFX);
            Destroy(recallInstance);
        }

        else
        {
            AudioManager.Instance.PlayAudioSFX(SpawnSFX);
            recallInstance = Instantiate(recallObject, transform.position, Quaternion.identity);
        }
    }
}