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
    private bool isPaused;


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
        //Initialize();
    }

    public void Initialize(GameObject _newLevel)
    {
        activeLevel = Instantiate(_newLevel, transform.position, Quaternion.identity);
        activeLevel.transform.Find("Walls").GetComponent<TilemapRenderer>().maskInteraction 
                = SpriteMaskInteraction.VisibleInsideMask;
        activeLevel.transform.Find("Floor").GetComponent<TilemapRenderer>().maskInteraction 
                = SpriteMaskInteraction.VisibleInsideMask;
        LevelExitHandler.OnExitReached += HandleLevelExit;
        SetStartingPoint(activeLevel.transform.Find("Start").transform);
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
        UnPauseGame();
    }

    private void HandleLevelExit()
    {
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
    
    public Transform GetSysMessagePoint(){ return sysMessagePoint;}
    public GameObject GetPlayerGO() { return playerGO; }
    public bool GetIsPaused() { return isPaused; }

}
