using System;
using Unity.VisualScripting;
using UnityEngine;

public class KeyHandler : MonoBehaviour, ICollectable
{
    public static Action OnKeyCollected;
    public void Collect()
    {
        SoundManager.PlaySound(SoundManager.Sound.pickupKey);
        OnKeyCollected?.Invoke();
        Destroy(gameObject);        
    }
}
