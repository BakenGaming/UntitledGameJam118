using System.Collections.Generic;
using UnityEngine;

public class LevelSelectManager : MonoBehaviour
{
    [SerializeField] private List<LevelButtonManager> levelButtons;

    public void Initialize()
    {
        foreach(LevelButtonManager _button in levelButtons)
            _button.Initialize();
    }
}
