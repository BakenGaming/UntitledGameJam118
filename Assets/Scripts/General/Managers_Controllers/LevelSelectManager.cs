using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectManager : MonoBehaviour
{
    [SerializeField] private List<LevelSO> levels;
    [SerializeField] private GameObject levelButtonParent;
    private List<LevelButtonManager> levelButtons;
    private LevelUnlockSystem levelUnlockSystem;
    public void Initialize()
    {
        levelButtons = new List<LevelButtonManager>();
        for(int i=0; i<levels.Count;i++)
        {
            GameObject newButton = Instantiate(GameAssets.i.pfLevelButton, levelButtonParent.transform);
            levelButtons.Add(newButton.GetComponent<LevelButtonManager>());
            newButton.GetComponent<LevelButtonManager>().Initialize(levels[i]);
        }
        //levelUnlockSystem = new LevelUnlockSystem(levels);
    }

    public void RefreshMenu()
    {
        Debug.Log("Menu Refreshed");
    }
}
