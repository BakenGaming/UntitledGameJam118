using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using CodeMonkey.Utils;

public class PlayerInputController_TopDown : MonoBehaviour, IInputHandler
{
    #region Events
    public static OnFireWeapon OnPlayerAttack;
    public static Action OnPauseGame;
    public static Action OnUnpauseGame;
    #endregion

    #region Variables
    public delegate void OnFireWeapon(Vector3 mousePos);
    private StatSystem _playerStats;
    private InputAction _move, _attack, _dash, _pause, _mousePos;
    private GameControls _controller;
    private Vector2 moveInput, lookDir;
    private Rigidbody2D playerRB;
    private Camera mainCam;

    #endregion

    #region Initialize
    public void Initialize()
    {
        _playerStats = GetComponent<IHandler>().GetStatSystem();    
        _controller = new GameControls();
        playerRB = GetComponent<Rigidbody2D>();
        mainCam = Camera.main;

        _move = _controller.TopDownControls.MoveInput;
        _move.Enable();

        _mousePos = _controller.TopDownControls.MousePosition;
        _mousePos.Enable();

        _dash = _controller.TopDownControls.Attack;
        _dash.performed += HandleDashInput;
        _dash.Enable();

        _pause = _controller.TopDownControls.Pause;
        _pause.performed += HandlePauseInput;
        _pause.Enable();

        _attack = _controller.TopDownControls.Attack;
        _attack.performed += HandleAttackInput;
        _attack.Enable();
    }

    private void OnDisable()
    {
        _move.Disable();
        _attack.Disable();
        _dash.Disable();
        _pause.Disable();
        _mousePos.Disable();
    }
    #endregion

    #region Input Handling
    private void HandleAttackInput(InputAction.CallbackContext context)
    {
        OnPlayerAttack(mainCam.ScreenToWorldPoint(_mousePos.ReadValue<Vector2>()));
    }
    private void HandleDashInput(InputAction.CallbackContext context)
    {
        
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
        if(!GameManager.i.GetIsPaused()) moveInput = _move.ReadValue<Vector2>();
        else playerRB.linearVelocity = Vector2.zero;
    }

    private void FixedUpdate() 
    {
        if(GameManager.i.GetIsPaused()) 
        {
            playerRB.linearVelocity = Vector2.zero;
            return;
        }
        
        Vector3 mousePosition = _mousePos.ReadValue<Vector2>();
        Vector3 screenPoint = mainCam.WorldToScreenPoint(transform.localPosition);

        Flip(mousePosition.x, screenPoint.x);
        
        Vector2 moveSpeed = moveInput.normalized;
        playerRB.linearVelocity = new Vector2(moveSpeed.x * _playerStats.GetMoveSpeed(), moveSpeed.y *.5f * _playerStats.GetMoveSpeed());    
    }

    #endregion

    #region Checks
    private void Flip(float mPosX, float sPtX)
    {
        if (mPosX < sPtX)
        {
            transform.Find("Sprite").GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            transform.Find("Sprite").GetComponent<SpriteRenderer>().flipX = false;
        }
    }
    #endregion
}
