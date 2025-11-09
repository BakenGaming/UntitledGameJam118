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
            Debug.Log("Handler");
            if(isKeyDoor)
            {
                Debug.Log("Is Key");
                UIController.i.OpenTextInformation("Space to Unlock");                
                _activeHandler.SetIsAtKeyDoor(true);
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(isKeyDoor)
        {
            UIController.i.CloseTextInformation();
            collision.GetComponent<IInputHandler>().SetIsAtKeyDoor(false);
            _activeHandler = null;
        }
    }
}
