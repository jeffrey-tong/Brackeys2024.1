using System;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button startGameBtn;
    [SerializeField] private string levelSceneName;

    private void Start()
    {
        startGameBtn.onClick.AddListener(OnStartGameBtnClicked);
    }

    private void OnStartGameBtnClicked()
    {
        TransitionManager.Instance.LoadScene(levelSceneName);
    }
}
