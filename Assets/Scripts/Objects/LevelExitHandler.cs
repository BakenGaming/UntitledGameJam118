using System;
using System.Collections;
using UnityEngine;

public class LevelExitHandler : MonoBehaviour
{
    public static event Action<LevelSO> OnExitReached;
    [SerializeField] private GameObject[] exitPortalSections;
    private IInputHandler _activeHandler;
    private float rotationSpeed = 150f;
    private bool rotationStarted;
    private LevelSO thisLevel;
    void Awake()
    {
        StartRotation();
    }

    public void SetThisLevel(LevelSO _level){thisLevel = _level;}

    private void StartRotation()
    {
        rotationStarted = true;
    }

    void Update()
    {
        if(rotationStarted)
        {
            for (int i = 0; i < exitPortalSections.Length; i++)
            {
                exitPortalSections[i].transform.Rotate(new Vector3(0, 0, rotationSpeed / (i+1)) * Time.deltaTime);
            }
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        _activeHandler = collision.GetComponent<IInputHandler>();
        if(_activeHandler != null)
        {
            UIController.i.OpenTextInformation("Space to Exit");
            _activeHandler.SetIsAtExit(true, this);
        }
        
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        _activeHandler.CancelExitHandler();
        _activeHandler = null;    
        UIController.i.CloseTextInformation();
    }

    public void ExitLevel()
    {
        SoundManager.PlaySound(SoundManager.Sound.levelEnd);
        StartCoroutine("DelayTime");
    }

    IEnumerator DelayTime()
    {
        yield return new WaitForSecondsRealtime(.75f);
        OnExitReached?.Invoke(thisLevel);
    }
}
