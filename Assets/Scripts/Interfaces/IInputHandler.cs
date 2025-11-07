using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInputHandler
{
    public void Initialize(PlayerHandler _handler);
    public void SetPickupItem(GameObject _obj, bool _isPlasma);
    public void SetIsAtKeyDoor(bool _isOnKeyDoor);
    public void SetIsAtSwitch(bool _isAtSwitch, SwitchHandler _handler);
    public void CancelSwitchHandler();
    public void SetIsAtExit(bool _isAtExit, LevelExitHandler _handler);
    public void CancelExitHandler();
    public void SetIsAtTeleporter(bool _isAtExit, TeleportHandler _handler);
    public void CancelTeleportHandler();
    public void MovePlayer(GameObject _location);
}
