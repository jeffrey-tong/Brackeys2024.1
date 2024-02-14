using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private PlayerLocomotion m_Locomotion;
    [SerializeField] private PlayerAbility m_Ability;
    [SerializeField] private CharacterData CharDataSO;

    private Door currentActivatable;

    private void Awake()
    {
        if (m_Locomotion == null) m_Locomotion = GetComponent<PlayerLocomotion>();
        if (m_Ability == null) m_Ability = GetComponent<PlayerAbility>();
    }

    private void Update()
    {
        float xInput = Input.GetAxisRaw("Horizontal");
        m_Locomotion.SetXInput(xInput);

        if (Input.GetKeyDown(KeyCode.Space))
            m_Locomotion.StartJump();

        if (Input.GetKeyUp(KeyCode.Space))
            m_Locomotion.StopJump();

        if (Input.GetKeyDown(KeyCode.W) && currentActivatable != null)
        {
            currentActivatable.EnterDoor(this);
        }
    }
    public void SetDoor(Door door)
    {
        this.currentActivatable = door;
    }

    public void ClearDoor(Door door)
    {
        if (this.currentActivatable == door)
            this.currentActivatable = null;
    }

    public CharacterData GetCharacterData() => CharDataSO;
}
