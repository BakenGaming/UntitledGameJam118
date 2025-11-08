using System.Collections.Generic;
using TMPro;
using UnityEngine;
using CodeMonkey.Utils;
using UnityEngine.Diagnostics;


public class LevelSelectManager : MonoBehaviour
{
    [SerializeField] private List<LevelSO> levels;
    [SerializeField] private GameObject levelButtonParent;
    [SerializeField] private GameObject lockedText;
    private List<LevelButtonManager> levelButtons;
    private LevelUnlockSystem levelUnlockSystem;
    public void Initialize()
    {
        HideLockedText();
        LevelExitHandler.OnExitReached += UnlockNextLevel;
        levelButtons = new List<LevelButtonManager>();
        for(int i=0; i<levels.Count;i++)
        {
            GameObject newButton = Instantiate(GameAssets.i.pfLevelButton, levelButtonParent.transform);
            levelButtons.Add(newButton.GetComponent<LevelButtonManager>());
            newButton.GetComponent<LevelButtonManager>().Initialize(levels[i], this);
        }
        levelButtons[0].UnlockLevel();
        levelUnlockSystem = new LevelUnlockSystem(levels);
        levelUnlockSystem.UnlockLevel(levels[0].levelName, true);
        //RefreshMenu();
    }

    public void RefreshMenu()
    {
        foreach(LevelButtonManager level in levelButtons)
        {
            if(levelUnlockSystem.CheckLevelLock(level.GetLevel().levelName) && !level.GetIsComplete()) 
                level.UnlockLevel();
        }
    }

    public void UnlockNextLevel(LevelSO _level)
    {
        foreach(LevelButtonManager level in levelButtons)
            if(_level.levelName == level.GetLevel().levelName) level.CompleteLevel();

        levelUnlockSystem.UnlockLevel(_level.nextLevel.levelName, true);
        RefreshMenu();
    }

    public void ShowLockedText(string _text, string _color)
    {
        lockedText.SetActive(true);
        lockedText.GetComponent<TextMeshProUGUI>().text = _text;
        lockedText.GetComponent<TextMeshProUGUI>().color = UtilsClass.GetColorFromString(_color);
    }
    public void HideLockedText(){lockedText.SetActive(false);}
}
