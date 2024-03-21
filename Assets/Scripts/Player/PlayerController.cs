using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private PlayerLocomotion m_Locomotion;
    [SerializeField] private PlayerAbility m_Ability;
    [SerializeField] private CharacterData CharDataSO;

    private ITrigger currentTrigger;

    private static PlayerController _current;
    public static PlayerController Current
    {
        get { return _current; }
        private set
        {
            _current = value;
            OnPlayerChanged?.Invoke();
        }
    }

    public static event Action OnPlayerChanged;

    private void Awake()
    {
        if (m_Locomotion == null) m_Locomotion = GetComponent<PlayerLocomotion>();
        if (m_Ability == null) m_Ability = GetComponent<PlayerAbility>();

        Current = this;

        Application.targetFrameRate = 60;
    }

    private void Update()
    {
        float xInput = Input.GetAxisRaw("Horizontal");
        m_Locomotion.SetXInput(xInput);

        if (Input.GetKeyDown(KeyCode.Space))
            m_Locomotion.StartJump();

        if (Input.GetKeyUp(KeyCode.Space))
            m_Locomotion.StopJump();

        if (Input.GetKeyDown(KeyCode.W) && currentTrigger != null)
        {
            currentTrigger.Trigger(this);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            TransitionManager.Instance.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (m_Ability != null && Input.GetKeyDown(m_Ability.GetAbilityKey()) && m_Ability.CanPerformAbility())
        {
            m_Ability.PerformAbility();
        }
    }
    public void SetDoor(ITrigger newTrigger)
    {
        this.currentTrigger = newTrigger;
    }

    public bool ClearTrigger(ITrigger trigger)
    {
        if (this.currentTrigger == trigger)
        {
            this.currentTrigger = null;
            return true;
        }

        return false;
    }

    public CharacterData GetCharacterData() => CharDataSO;
    public PlayerLocomotion GetLocomotion() => m_Locomotion;
}
