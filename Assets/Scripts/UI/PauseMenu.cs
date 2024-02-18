using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private GameObject settingsMenuPanel;
    [SerializeField] private Button resumeBtn;
    [SerializeField] private Button exitBtn;

    [SerializeField] private string mainMenuScene;

    private bool isGamePaused = false;

    private void Start()
    {
        resumeBtn.onClick.AddListener(OnResumeBtnClicked);
        exitBtn.onClick.AddListener(OnExitBtnClicked);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) || (Input.GetKeyDown(KeyCode.Escape)))
        {
            TogglePause();
        }
    }

    private void OnExitBtnClicked()
    {
        Time.timeScale = 1;
        TransitionManager.Instance.LoadScene(mainMenuScene);
    }

    private void OnResumeBtnClicked()
    {
        if (isGamePaused)
            TogglePause();
    }

    private void TogglePause()
    {
        isGamePaused = !isGamePaused;

        Time.timeScale = isGamePaused ? 0 : 1;
        pauseMenu.SetActive(isGamePaused);
        pauseMenuPanel.SetActive(true);
        settingsMenuPanel.SetActive(false);
    }
}