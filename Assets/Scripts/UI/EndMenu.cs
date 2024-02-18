using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndMenu : MonoBehaviour
{
    [SerializeField] private Button mainMenuGameBtn;
    [SerializeField] private string levelSceneName;

    private void Start()
    {
        mainMenuGameBtn.onClick.AddListener(OnMainMenuBtnClicked);
    }

    private void OnMainMenuBtnClicked()
    {
        TransitionManager.Instance.LoadScene(levelSceneName);
    }
}
