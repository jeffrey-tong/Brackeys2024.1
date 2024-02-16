using UnityEngine;

public abstract class PlayerAbility : MonoBehaviour
{
    [SerializeField] private KeyCode AbilityCode; 
    public abstract bool CanPerformAbility();
    public abstract void PerformAbility();
    public KeyCode GetAbilityKey() => AbilityCode;
}
