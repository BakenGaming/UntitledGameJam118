using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.EventSystems;

public class LevelButtonManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static event Action OnLevelSelected;
    private LevelSelectManager _lsManager;
    private GameObject levelButton;
    private LevelSO level;
    private bool isLocked, isComplete;
    public void Initialize(LevelSO _level, LevelSelectManager _ls)
    {
        _lsManager = _ls;
        level = _level;
        levelButton = transform.Find("Button").gameObject;
        levelButton.GetComponent<Image>().sprite = GameAssets.i.levelSelectBG;
        levelButton.transform.Find("LevelName").GetComponent<TextMeshProUGUI>().text = _level.levelName;
        levelButton.transform.Find("Difficulty").GetComponent<Image>().sprite = GameAssets.i.difficultyMarkers[_level.difficultyRating];
        levelButton.transform.Find("LevelImage").GetComponent<Image>().sprite = GameAssets.i.lockedImage;
        isLocked = true;
        isComplete = false;
    } 

    public void SelectLevel()
    {
        if(!isLocked)
        {
            GameManager.i.Initialize(level);
            OnLevelSelected?.Invoke();
        }
        else SoundManager.PlaySound(SoundManager.Sound.uiLocked);

        
    }

    public void UnlockLevel()
    {
        levelButton.transform.Find("LevelImage").GetComponent<Image>().sprite = GameAssets.i.unlockedImage;
        isLocked = false;
    }

    public void CompleteLevel()
    {
        Debug.Log("Level Completed");
        levelButton.transform.Find("LevelImage").GetComponent<Image>().sprite = GameAssets.i.completeImage;
        isComplete = true;
    }

    public LevelSO GetLevel(){return level;}
    public bool GetIsComplete(){return isComplete;}

    public void OnPointerEnter(PointerEventData eventData)
    {
        SoundManager.PlaySound(SoundManager.Sound.uiClick);
        levelButton.GetComponent<Image>().sprite = GameAssets.i.levelSelectedBG;
        if(isLocked) _lsManager.ShowLockedText("Locked", "9E2835");
        if(isComplete) _lsManager.ShowLockedText("Complete", "63C64D");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        levelButton.GetComponent<Image>().sprite = GameAssets.i.levelSelectBG;
        _lsManager.HideLockedText();
    }
}
