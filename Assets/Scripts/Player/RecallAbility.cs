using UnityEngine;

public class RecallAbility : PlayerAbility
{
    [SerializeField] private GameObject recallObject;

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
            Destroy(recallInstance);
        }

        else
        {
            recallInstance = Instantiate(recallObject, transform.position, Quaternion.identity);
        }
    }
}