using Unity.VisualScripting;
using UnityEngine;

public class DoorHandler : MonoBehaviour
{
    [SerializeField] private GameObject _doorObject;
    [SerializeField] private bool isKeyDoor;
    private IInputHandler _activeHandler;
    void OnEnable()
    {
        PlayerInputController_TopDown.OnKeyAction += Unlock;
    }
    void OnDisable()
    {
        PlayerInputController_TopDown.OnKeyAction -= Unlock;
    }
    public void SwitchActivated()
    {
        if(!isKeyDoor)
        {
            _doorObject.SetActive(false);
            SoundManager.PlaySound(SoundManager.Sound.unlockDoor);
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    public void Unlock(bool _isUsed)
    {
        if(!_isUsed) return;

        if (isKeyDoor) 
        { 
            SoundManager.PlaySound(SoundManager.Sound.unlockDoor);
            _doorObject.SetActive(false); 
            GetComponent<BoxCollider2D>().enabled = false; 
        }
    }

    public void LevelReset()
    {
        _doorObject.SetActive(true);
        if(isKeyDoor) GetComponent<BoxCollider2D>().enabled = true;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        _activeHandler = collision.GetComponent<IInputHandler>();
        if(_activeHandler != null)
        {
            if(isKeyDoor)
            {
                if(_activeHandler.GetHasKey())
                    UIController.i.OpenTextInformation("Space to Unlock");                
                else
                    UIController.i.OpenTextInformation("Find the Key");    
                _activeHandler.SetIsAtKeyDoor(true);
            }
            else
                UIController.i.OpenTextInformation("Find the Switch");
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        UIController.i.CloseTextInformation();
        if(isKeyDoor) collision.GetComponent<IInputHandler>().SetIsAtKeyDoor(false);
        _activeHandler = null;

    }
}
