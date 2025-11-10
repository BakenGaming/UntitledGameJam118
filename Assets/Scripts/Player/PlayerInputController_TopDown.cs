using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using CodeMonkey.Utils;
using Unity.VisualScripting;

public class PlayerInputController_TopDown : MonoBehaviour, IInputHandler
{
    #region Events
    public static OnFireWeapon OnPlayerAttack;
    public static Action OnPauseGame;
    public static Action OnUnpauseGame;
    public static Action<int> OnUpdatePlasmaCount;
    public static Action<bool> OnKeyAction;
    #endregion

    #region Variables
    public delegate void OnFireWeapon(Vector3 mousePos);
    private StatSystem _playerStats;
    private InputAction _move, _action, _pause, _pickUp;
    private GameControls _controller;
    private Vector2 moveInput, lookDir;
    private Rigidbody2D playerRB;
    private Camera mainCam;
    private PlayerHandler handler;
    private GameObject _pickupObject;
    private SwitchHandler _activeSwitchHandler;
    private LevelExitHandler _activeExitHandler;
    private TeleportHandler _activeTeleportHandler;
    private bool plasmaObject, isStandingByKeyDoor, hasKey, isStandingBySwitch, isAtExit, isAtTeleporter;
    private int plasmaCount = 3;

    #endregion

    #region Initialize
    public void Initialize(PlayerHandler _handler)
    {
        _playerStats = GetComponent<IHandler>().GetStatSystem();    
        _controller = new GameControls();
        handler = _handler;
        plasmaCount = 3;
        OnUpdatePlasmaCount?.Invoke(plasmaCount);
        playerRB = GetComponent<Rigidbody2D>();
        mainCam = Camera.main;

        _move = _controller.TopDownControls.MoveInput;
        _move.Enable();

        _pause = _controller.TopDownControls.Pause;
        _pause.performed += HandlePauseInput;
        _pause.Enable();

        _action = _controller.TopDownControls.Action;
        _action.performed += HandleActionInput;
        _action.Enable();

        _pickUp = _controller.TopDownControls.Pickup;
        _pickUp.performed += HandlePickup; 
        _pickUp.Enable();

        KeyHandler.OnKeyCollected += SetHasKey;
    }


    private void OnDisable()
    {
        _move.Disable();
        _action.Disable();
        _pause.Disable();
        _pickUp.Disable();

        KeyHandler.OnKeyCollected -= SetHasKey;
    }
    #endregion

    #region Input Handling
    private void HandleActionInput(InputAction.CallbackContext context)
    {
        if(GameManager.i.GetIsPaused()) return;

        if(isStandingByKeyDoor)
        {
            if(hasKey) { OnKeyAction?.Invoke(true); hasKey = false; }
        }
        else if(isStandingBySwitch)
        {
            _activeSwitchHandler.ActivateSequence();
            GameManager.i.PauseGame();
        }
        else if(isAtExit)
        {
            _activeExitHandler.ExitLevel();
        }
        else if(isAtTeleporter)
        {
            SoundManager.PlaySound(SoundManager.Sound.teleport);
            _activeTeleportHandler.Teleport();
            GameManager.i.PauseGame();
        }
        else
        {
            if(plasmaCount == 0) return;
            GameObject newPlasma = Instantiate (GameAssets.i.pfPlasma, transform.position, Quaternion.identity);
            handler.UpdateLightMaskSize(false);
            plasmaCount--;
            SoundManager.PlaySound(SoundManager.Sound.dropPlasma);
            OnUpdatePlasmaCount?.Invoke(plasmaCount);
        }
    }
    private void HandlePickup(InputAction.CallbackContext context)
    {
        if(_pickupObject != null)
        {
            if(plasmaObject) 
            { 
                handler.UpdateLightMaskSize(true); 
                plasmaCount++; 
                OnUpdatePlasmaCount?.Invoke(plasmaCount);
            }
            _pickupObject.GetComponent<IPickupHandler>().HandlePickup();
        }
    }
    public void SetPickupItem(GameObject _object, bool _isPlasma)
    {
        plasmaObject = _isPlasma;
        _pickupObject = _object;
    }

    public void SetHasKey()
    {
        hasKey = true;
        OnKeyAction?.Invoke(false);
    }

    public void SetIsAtKeyDoor(bool _isAtDoor)
    {
        isStandingByKeyDoor = _isAtDoor;
    }

    public void SetIsAtSwitch(bool _isAtSwitch, SwitchHandler _handler)
    {
        isStandingBySwitch = _isAtSwitch;
        _activeSwitchHandler = _handler;
    }

    public void CancelSwitchHandler()
    {
        isStandingBySwitch = false; 
        _activeSwitchHandler = null;
        if(GameManager.i.GetIsPaused()) GameManager.i.UnPauseGame();
    }
    public void SetIsAtExit(bool _isAtExit, LevelExitHandler _handler)
    {
        isAtExit = _isAtExit;
        _activeExitHandler = _handler;
    }

    public void CancelExitHandler()
    {
        isAtExit = false; 
        _activeExitHandler = null;
        if(GameManager.i.GetIsPaused()) GameManager.i.UnPauseGame();
    }

    public void SetIsAtTeleporter(bool _isAtExit, TeleportHandler _handler)
    {
        isAtTeleporter = _isAtExit;
        _activeTeleportHandler = _handler;
    }

    public void CancelTeleportHandler()
    {
        isAtTeleporter = false; 
        _activeTeleportHandler = null;
        if(GameManager.i.GetIsPaused()) GameManager.i.UnPauseGame();
    }

    public void MovePlayer(GameObject _location)
    {
        transform.position = _location.transform.position;
    }
    private void HandlePauseInput(InputAction.CallbackContext context)
    {
        if(!GameManager.i.GetIsPaused()) OnPauseGame?.Invoke();
        else OnUnpauseGame?.Invoke();

    }

    #endregion

    #region Loop
    private void Update() 
    {
        if(GameManager.i.GetIsPaused()) playerRB.linearVelocity = Vector2.zero;
        else moveInput = _move.ReadValue<Vector2>();
    }

    private void FixedUpdate() 
    {
        if(GameManager.i.GetIsPaused()) 
        {
            playerRB.linearVelocity = Vector2.zero;
            return;
        }
        
        Vector2 moveSpeed = moveInput.normalized;
        playerRB.linearVelocity = new Vector2(moveSpeed.x * _playerStats.GetMoveSpeed(), moveSpeed.y *.5f * _playerStats.GetMoveSpeed());    
    }

    #endregion

    #region Checks
    public bool GetHasKey(){return hasKey;}
    #endregion
}
