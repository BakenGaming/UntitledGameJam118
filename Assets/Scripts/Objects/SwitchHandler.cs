using System.Collections;
using UnityEngine;

public class SwitchHandler : MonoBehaviour
{
    [SerializeField] private DoorHandler _doorToControl;
    [SerializeField] private Sprite[] activationSprites;
    [SerializeField] private SpriteRenderer _sr;
    private IInputHandler _activeHandler;

    void OnTriggerEnter2D(Collider2D collision)
    {
        _activeHandler = collision.GetComponent<IInputHandler>();
        if(_activeHandler != null)
        {
            UIController.i.OpenTextInformation("Space to Acivate");
            _activeHandler.SetIsAtSwitch(true, this);
        }
        
    }

    public void ActivateSequence()
    {
        StartCoroutine("ActivateDoor");
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        _activeHandler.CancelSwitchHandler();
        _activeHandler = null;    
        UIController.i.CloseTextInformation();
    }

    IEnumerator ActivateDoor()
    {
        for (int i = 0; i < activationSprites.Length; i++)
        {
            _sr.sprite = activationSprites[i];
            SoundManager.PlaySound(SoundManager.Sound.activateSwitch);
            yield return new WaitForSeconds(.4f);
        }
        _activeHandler.CancelSwitchHandler();
        _doorToControl.SwitchActivated();
    }
}
