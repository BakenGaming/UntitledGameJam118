using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;


public class UIController : MonoBehaviour
{
    #region Setup and Variables
    private static UIController _i;
    public static UIController i { get { return _i; } }
    [Header("MENUS")]
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject creditsScreen;
    [SerializeField] private GameObject mainMenuButtons;
    [SerializeField] private GameObject creditsButton;
    [SerializeField] private GameObject titleGraphic;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject openingScene;
    [SerializeField] private GameObject closingScene;
    [SerializeField] private GameObject levelSelectMenu;
    [SerializeField] private bool isMainMenu;

    [Header("UI PANELS")]
    [SerializeField] private GameObject fadeScreen;
    [SerializeField] private GameObject levelName;
    [SerializeField] private GameObject textInformationPanel;
    [SerializeField] private TextMeshProUGUI textInformationPanelText;
    [SerializeField] private GameObject tutorialMenu;
    
    [Header("GAME UI")]
    [SerializeField] private GameObject plasmaObject;
    [SerializeField] private TextMeshProUGUI plasmaCount;
    [SerializeField] private GameObject keyImage;
    private string _currentTutorial="";

    private void OnEnable() 
    {
        if (isMainMenu) Initialize();
    }
    private void OnDisable() 
    {
        PlayerInputController_TopDown.OnUpdatePlasmaCount -= UpdateGameUI;
        PlayerInputController_TopDown.OnKeyAction -= UpdateKeyUI;
        LevelExitHandler.OnExitReached -= FadeToBlack;
        FadeManager.OnFadeToBlackComplete -= ActivateLevelSelectMenu;
        FadeManager.OnFadeFromBlackComplete -= DeactivateLevelSelectMenu;
    }
    public void Initialize()
    {
        settingsMenu.SetActive(false);
        GetComponent<VolumeSettings>().Initialize(); 

        if(isMainMenu)
        {
            creditsScreen.SetActive(false);
            openingScene.SetActive(false);
            creditsButton.SetActive(true);
            mainMenuButtons.SetActive(true);
            titleGraphic.SetActive(true);
            int track = UnityEngine.Random.Range(0, GameAssets.i.musicTrackArray.Length);
            SoundManager.Music newMusic = GameAssets.i.musicTrackArray[track].music;
            SoundManager.PlayMusic(newMusic);
        }

        _i = this;
        
        if(!isMainMenu)
        {
            pauseMenu.SetActive(false);
            CloseTutorialMenu();
            CloseTextInformation();
            ActivateLevelSelectMenu();
        }
    }
    #endregion
    #region Menus
    public void OpenPauseMenu()
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
        if(isMainMenu)
        {
            creditsButton.SetActive(false);
            mainMenuButtons.SetActive(false);
            titleGraphic.SetActive(false);
        }
        GetComponent<VolumeSettings>().SettingsMenuOpened();
    }
    public void CloseSettingsMenu()
    {
        settingsMenu.SetActive(false);
        if(isMainMenu)
        {
            creditsButton.SetActive(true);
            mainMenuButtons.SetActive(true);
            titleGraphic.SetActive(true);
        }
    }
    public void OpenCreditsScreen()
    {
        if(isMainMenu)
        {
            creditsButton.SetActive(false);
            mainMenuButtons.SetActive(false);
        }
        creditsScreen.SetActive(true);
    }
    public void CloseCreditsScreen()
    {
        creditsScreen.SetActive(false);
        if(isMainMenu)
        {
            creditsButton.SetActive(true);
            mainMenuButtons.SetActive(true);
        }
    }
    private void ActivateLevelSelectMenu()
    {
        fadeScreen.SetActive(false);
        CloseTutorialMenu();
        GameManager.i.PauseGame();
        keyImage.SetActive(false);
        plasmaObject.SetActive(false);
        levelName.SetActive(false);
        levelSelectMenu.SetActive(true);
        int track = UnityEngine.Random.Range(0, GameAssets.i.musicTrackArray.Length);
        SoundManager.Music newMusic = GameAssets.i.musicTrackArray[track].music;
        SoundManager.PlayMusic(newMusic);
        if(!GameManager.i.GetGameHasStarted())
            levelSelectMenu.GetComponent<LevelSelectManager>().Initialize();
        LevelButtonManager.OnLevelSelected += DeactivateLevelSelectMenu;
    }
    private void DeactivateLevelSelectMenu(LevelSO _level)
    {
        FadeFromBlack();
        plasmaObject.SetActive(true);
        levelSelectMenu.SetActive(false);
        levelName.SetActive(true);
        levelName.GetComponent<TextMeshProUGUI>().text = _level.levelName;
    }

    private void FadeToBlack(LevelSO _level)
    {
        Debug.Log("Fade To Black");
        fadeScreen.SetActive(true);
        fadeScreen.GetComponent<FadeManager>().InitiateFade(false);
    }
    private void FadeFromBlack() 
    {
        Debug.Log("Fade from Black");
        fadeScreen.SetActive(true);
        fadeScreen.GetComponent<FadeManager>().InitiateFade(true);
    }
    private void FadeComplete(LevelSO _unused)
    {
        Debug.Log("Fade Complete"); 
        fadeScreen.SetActive(false); 
        GameManager.i.UnPauseGame();
    }

    private void CompleteGame()
    {
        GameManager.i.PauseGame();
        closingScene.SetActive(true);
        OpeningDialogHandler.i.StartDialog();
    }
    #endregion
    #region GameUIRelated
    public void PrepareUI()
    {
        PlayerInputController_TopDown.OnUpdatePlasmaCount += UpdateGameUI;
        PlayerInputController_TopDown.OnKeyAction += UpdateKeyUI;
        LevelExitHandler.OnExitReached += FadeToBlack;
        LevelExitHandler.OnFinalLevelCompleted += CompleteGame;
        FadeManager.OnFadeToBlackComplete += ActivateLevelSelectMenu;
        FadeManager.OnFadeFromBlackComplete += FadeComplete;
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
    public void StartOpeningScene()
    {
        creditsButton.SetActive(false);
        mainMenuButtons.SetActive(false);
        titleGraphic.SetActive(false);
        openingScene.SetActive(true);
        OpeningDialogHandler.i.StartDialog();
    }
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
    #region Tutorials
    public void OpenTutorialMenu(string _menuName)
    {
        tutorialMenu.SetActive(true);
        _currentTutorial = _menuName;
        tutorialMenu.transform.Find(_menuName).gameObject.SetActive(true);
    }

    public void CloseTutorialMenu()
    {
        if(_currentTutorial != "") tutorialMenu.transform.Find(_currentTutorial).gameObject.SetActive(false);
        tutorialMenu.SetActive(false);
        _currentTutorial = "";
    }
    #endregion
}
