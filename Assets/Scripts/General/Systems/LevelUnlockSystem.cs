using System.Collections.Generic;
using UnityEngine;

public class LevelUnlockSystem
{
    public Dictionary<string, bool> levelLock;
    public LevelUnlockSystem (List<LevelSO> levels)
    {
        levelLock = new Dictionary<string, bool>();
        foreach(LevelSO level in levels)
        {
            levelLock.Add(level.levelName, false);
        }
        //levelLock[levels[0].name] = true;
    }

    public void UnlockLevel(string _name, bool _unlocked)
    {
       levelLock[_name] = _unlocked; 
    }

    public bool CheckLevelLock(string _name)
    {
        return levelLock[_name];
    }
}
