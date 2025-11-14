using UnityEngine;

[CreateAssetMenu(menuName ="Level")]
public class LevelSO : ScriptableObject
{
    public string levelName;
    public string tutorialName;
    public GameObject levelPrefab;
    public LevelSO nextLevel;
    public int difficultyRating;
    public bool isTutorial;
    public bool isFinalLevel;
}
