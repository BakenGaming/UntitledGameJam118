using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class LevelButtonManager : MonoBehaviour
{
    public static event Action OnLevelSelected;
    [SerializeField] private LevelSO _level;
    [SerializeField] private GameObject levelButton;

    public void Initialize()
    {
        levelButton.GetComponent<Image>().sprite = GameAssets.i.levelSelectBG;
        levelButton.transform.Find("LevelName").GetComponent<TextMeshProUGUI>().text = _level.levelName;
        levelButton.transform.Find("Difficulty").GetComponent<Image>().sprite = GameAssets.i.difficultyMarkers[_level.difficultyRating];
    } 

    public void SelectLevel()
    {
        GameManager.i.Initialize(_level.levelPrefab);
        OnLevelSelected?.Invoke();
    }
}
