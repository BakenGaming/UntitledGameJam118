using System;
using Unity.VisualScripting;
using UnityEngine;

public class KeyHandler : MonoBehaviour, ICollectable
{
    public static Action OnKeyCollected;
    public void Collect()
    {
        OnKeyCollected?.Invoke();
        Destroy(gameObject);        
    }
}
