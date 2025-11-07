using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class UIController : MonoBehaviour
{
    #region Setup and Variables
    private static UIController _i;
    public static UIController i { get { return _i; } }
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject creditsScreen;
    [SerializeField] private GameObject levelSelectMenu;
    [SerializeField] private GameObject textInformationPanel;
    [SerializeField] private TextMeshProUGUI textInformationPanelText;
    [SerializeField] private bool isMainMenu;
    [SerializeField] private GameObject plasmaObject;
    [SerializeField] private TextMeshProUGUI plasmaCount;
    [SerializeField] private GameObject keyImage;

    private void OnEnable() 
    {
        if (isMainMenu) Initialize();
    }
    private void OnDisable() 
    {
        PlayerInputController_TopDown.OnUpdatePlasmaCount -= UpdateGameUI;
        PlayerInputController_TopDown.OnKeyAction -= UpdateKeyUI;
        LevelExitHandler.OnExitReached -= ActivateLevelSelectMenu;
        LevelButtonManager.OnLevelSelected -= DeactivateLevelSelectMenu;
    }
    public void Initialize()
    {
        //GetComponent<VolumeSettings>().Initialize();        
        //if(!isMainMenu) pauseMenu.SetActive(false);
        //else creditsScreen.SetActive(false);
        
        //settingsMenu.SetActive(false);
        _i = this;
        CloseTextInformation();
        ActivateLevelSelectMenu();
    }
    #endregion
    #region Menus
    private void OpenPauseMenu()
    {
        pauseMenu.SetActive(true);
        GameManager.i.PauseGame();
    }
    public void ClosePauseMenu()
    {
        pauseMenu.SetActive(false);
        GameManager.i.UnPauseGame();
    }
    public void OpenSettingsMenu()
    {
        settingsMenu.SetActive(true);
        GetComponent<VolumeSettings>().SettingsMenuOpened();
    }
    public void CloseSettingsMenu()
    {
        settingsMenu.SetActive(false);
    }
    public void OpenCreditsScreen()
    {
        creditsScreen.SetActive(true);
    }
    public void CloseCreditsScreen()
    {
        creditsScreen.SetActive(false);
    }
    private void ActivateLevelSelectMenu()
    {
        GameManager.i.PauseGame();
        keyImage.SetActive(false);
        plasmaObject.SetActive(false);
        levelSelectMenu.SetActive(true);
        levelSelectMenu.GetComponent<LevelSelectManager>().Initialize();
        LevelButtonManager.OnLevelSelected += DeactivateLevelSelectMenu;
    }
    private void DeactivateLevelSelectMenu()
    {
        plasmaObject.SetActive(true);
        levelSelectMenu.SetActive(false);
    }
    #endregion
    #region GameUIRelated
    public void PrepareUI()
    {
        PlayerInputController_TopDown.OnUpdatePlasmaCount += UpdateGameUI;
        PlayerInputController_TopDown.OnKeyAction += UpdateKeyUI;
        LevelExitHandler.OnExitReached += ActivateLevelSelectMenu;
    }
    private void UpdateGameUI(int _count)
    {
        plasmaCount.text = _count.ToString();
    }

    private void UpdateKeyUI(bool _used)
    {
        if(_used) keyImage.SetActive(false);
        else keyImage.SetActive(true);
    }

    public void OpenTextInformation(string _message)
    {
        textInformationPanel.SetActive(true);
        textInformationPanelText.text = _message;
    }

    public void CloseTextInformation()
    {
        textInformationPanel.SetActive(false);
    }
    #endregion
    #region Menu Actions
    public void StartGame()
    {
        SceneController.StartGame();
    }

    public void RestartGame()
    {
        SceneController.StartGame();
    }
    public void BackToMainMenu()
    {
        SceneController.LoadMainMenu();
    }

    public void ExitGame()
    {
        SceneController.ExitGame();
    }
    #endregion

}
