using System;
using UnityEngine;

public class PlasmaHandler : MonoBehaviour, IPickupHandler
{
    public event Action OnAbleToPickup;
    public event Action OnUnAbleToPickup;
    private IInputHandler _activeHandler;

    public void HandlePickup()
    {
        if(_activeHandler != null)
        {
            //_activeHandler.ObjectPickupHandler();
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        _activeHandler = collision.GetComponent<IInputHandler>();
        if(_activeHandler != null)
        {
            _activeHandler.SetPickupItem(gameObject, true);
            UIController.i.OpenTextInformation("F to Pickup");
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        UIController.i.CloseTextInformation();
        _activeHandler.SetPickupItem(null, false);
        _activeHandler = null;
    }
}
