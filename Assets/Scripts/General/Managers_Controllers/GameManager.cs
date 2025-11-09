using CodeMonkey.MonoBehaviours;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    #region Variables
    private static GameManager _i;
    public static GameManager i { get { return _i; } }
    [SerializeField] private Transform sysMessagePoint;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private UIController gameUIController;
    private GameObject playerGO, activeLevel;
    private bool isPaused, gameHasStarted;


    #endregion

    #region Initialize
    void OnDisable()
    {
        LevelExitHandler.OnExitReached -= HandleLevelExit;
    }
    private void Awake() 
    {
        _i = this;  
        SetupObjectPools();  
        gameUIController.Initialize();
        gameHasStarted = true;
        //Initialize();
    }

    public void Initialize(LevelSO _level)
    {
        activeLevel = Instantiate(_level.levelPrefab, transform.position, Quaternion.identity);
        activeLevel.transform.Find("Walls").GetComponent<TilemapRenderer>().maskInteraction 
                = SpriteMaskInteraction.VisibleInsideMask;
        activeLevel.transform.Find("Floor").GetComponent<TilemapRenderer>().maskInteraction 
                = SpriteMaskInteraction.VisibleInsideMask;
        activeLevel.transform.Find("End").GetComponentInChildren<LevelExitHandler>().SetThisLevel(_level);
        LevelExitHandler.OnExitReached += HandleLevelExit;
        SetStartingPoint(activeLevel.transform.Find("Start").transform);
        if(_level.isTutorial) gameUIController.OpenTutorialMenu(_level.tutorialName);
    }

    public void SetStartingPoint(Transform _spawnPoint)
    {
        spawnPoint = _spawnPoint;
        SpawnPlayerObject();
    }

    private void SpawnPlayerObject()
    {
        playerGO = Instantiate(GameAssets.i.pfPlayerObject, spawnPoint);
        playerGO.transform.parent = null;
        playerGO.GetComponent<IHandler>().Initialize();
        CameraController.i.CameraSetup(playerGO);
        gameUIController.PrepareUI();
        PlayerInputController_TopDown.OnPauseGame += HandlePauseGame;
        PlayerInputController_TopDown.OnUnpauseGame += HandleUnpauseGame;
        int track = Random.Range(0, GameAssets.i.musicTrackArray.Length);
        SoundManager.Music newMusic = GameAssets.i.musicTrackArray[track].music;
        Debug.Log($"[0,{GameAssets.i.musicTrackArray.Length}] = {track} >> {newMusic}");
        SoundManager.PlayMusic(newMusic);
        SoundManager.AdjustVolumeDown();
        UnPauseGame();
    }

    private void HandleLevelExit(LevelSO _level)
    {
        SoundManager.StopMusic();
        Destroy(activeLevel.gameObject);
        Destroy(playerGO.gameObject);
    }

    public void SetupObjectPools()
    {
        //Do the below for all objects that will need pooled for use
        //ObjectPooler.SetupPool(OBJECT, SIZE, "NAME") == Object is pulled from GameAssets, Setup object with a SO that contains size and name
        
        //The below is placed in location where object is needed from pool
        //==============================
        //PREFAB_SCRIPT instance = ObjectPooler.DequeueObject<PREFAB_SCRIPT>("NAME");
        //instance.gameobject.SetActive(true);
        //instance.Initialize();
        //==============================
    }
    #endregion

    public void PauseGame(){if(isPaused) return; else isPaused = true;}
    public void UnPauseGame(){if(isPaused) isPaused = false; else return;}

    private void HandlePauseGame(){gameUIController.OpenPauseMenu();}
    private void HandleUnpauseGame(){gameUIController.ClosePauseMenu();}
    
    public Transform GetSysMessagePoint(){ return sysMessagePoint;}
    public GameObject GetPlayerGO() { return playerGO; }
    public bool GetIsPaused() { return isPaused; }
    public bool GetGameHasStarted(){return gameHasStarted;}

}
