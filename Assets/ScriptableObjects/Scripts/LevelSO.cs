using UnityEngine;

[CreateAssetMenu(menuName ="Level")]
public class LevelSO : ScriptableObject
{
    public string levelName;
    public GameObject levelPrefab;
    public int difficultyRating;
    public bool isTutorial;
}
