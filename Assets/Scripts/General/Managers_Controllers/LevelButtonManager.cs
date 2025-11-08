using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class LevelButtonManager : MonoBehaviour
{
    public static event Action OnLevelSelected;
    private GameObject levelButton;
    private LevelSO level;

    public void Initialize(LevelSO _level)
    {
        level = _level;
        levelButton = transform.Find("Button").gameObject;
        levelButton.GetComponent<Image>().sprite = GameAssets.i.levelSelectBG;
        levelButton.transform.Find("LevelName").GetComponent<TextMeshProUGUI>().text = _level.levelName;
        levelButton.transform.Find("Difficulty").GetComponent<Image>().sprite = GameAssets.i.difficultyMarkers[_level.difficultyRating];
    } 

    public void SelectLevel()
    {
        GameManager.i.Initialize(level.levelPrefab);
        OnLevelSelected?.Invoke();
    }
}
