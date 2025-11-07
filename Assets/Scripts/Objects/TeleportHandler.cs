using UnityEngine;
using System;
using UnityEditor;

[Serializable]
public enum TeleporterClass
{A,B,C,D}
public class TeleportHandler : MonoBehaviour
{
    [SerializeField] private Color[] availableColors;
    [SerializeField] private TeleporterClass _class;
    [SerializeField] private GameObject linkedTeleporter;
    [SerializeField] private ParticleSystem _ps;
    [SerializeField] private SpriteRenderer sr;
    private IInputHandler _activeHandler;
    private bool justTeleportedHere;
    private ParticleSystem.MainModule _psMain;

    void Awake()
    {
        _psMain = _ps.main;
        justTeleportedHere = false;
        switch (_class)
        {
            case TeleporterClass.A:
                sr.color = availableColors[0];
                _psMain.startColor = availableColors[0];
            break;
            case TeleporterClass.B:
                sr.color = availableColors[1];
                _psMain.startColor = availableColors[1];
            break;
            case TeleporterClass.C:
                sr.color = availableColors[2];
                _psMain.startColor = availableColors[2];                
            break;
            case TeleporterClass.D:
                sr.color = availableColors[3];
                _psMain.startColor = availableColors[3];                
            break;
        }
    }

    public void TempTeleportDisable()
    {
        justTeleportedHere = true;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        _activeHandler = collision.GetComponent<IInputHandler>();
        if(_activeHandler != null)
        {
            UIController.i.OpenTextInformation("Space to Teleport");
            _activeHandler.SetIsAtTeleporter(true, this);
        }
        
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        justTeleportedHere = false;
        _activeHandler.CancelTeleportHandler();
        _activeHandler = null;    
        UIController.i.CloseTextInformation();
    }

    public void Teleport()
    {
            linkedTeleporter.GetComponent<TeleportHandler>().TempTeleportDisable();
            _activeHandler.MovePlayer(linkedTeleporter);
    }
}
