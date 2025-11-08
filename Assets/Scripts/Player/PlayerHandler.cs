using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerHandler : MonoBehaviour, IHandler
{
    #region Variables
    [SerializeField] private PlayerStatsSO playerStatsSO;
    [SerializeField] private SpriteMask lightMask;

    private StatSystem _statSystem;
    #endregion
    #region Initialize
    public void Initialize()
    {
        SetupPlayer();
    }

    #endregion

    #region Get Functions
    public StatSystem GetStatSystem()
    {
        return _statSystem;
    }

    #endregion

    #region Handle Player Functions

    public void HandleDeath()
    {
        throw new System.NotImplementedException();
    }

    public void UpdateHealth()
    {
        throw new System.NotImplementedException();
    }

    public void UpdateLightMaskSize(bool _increase)
    {
        if(_increase) lightMask.transform.localScale = new Vector2(lightMask.transform.localScale.x + .5f,
            lightMask.transform.localScale.y + .5f);
        else lightMask.transform.localScale = new Vector2(lightMask.transform.localScale.x - .5f,
            lightMask.transform.localScale.y - .5f);
    }
    #endregion

    #region Player Setup
    private void SetupPlayer()
    {
        _statSystem = new StatSystem(playerStatsSO);
        GetComponent<IInputHandler>().Initialize(this);
    }
    #endregion
}
