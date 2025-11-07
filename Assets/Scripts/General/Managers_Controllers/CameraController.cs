using UnityEngine;

public class CameraController : MonoBehaviour
{
    private static CameraController _i;
    public static CameraController i { get { return _i; } }
    private Camera mainCam;
    private GameObject followObject;
    private Vector3 newPosition;
    private bool cameraActive= false;
    void OnEnable()
    {
        _i = this;
        mainCam = Camera.main;
    }

    void OnDisable()
    {
        LevelExitHandler.OnExitReached -= DisableCameraFollow;
    }

    public void CameraSetup(GameObject _followObject)
    {
        LevelExitHandler.OnExitReached += DisableCameraFollow;
        followObject = _followObject;
        newPosition = new Vector3(followObject.transform.position.x, followObject.transform.position.y, -10f);
        mainCam.transform.position = newPosition;
        cameraActive = true;
        
    }

    void LateUpdate()
    {
        if(!cameraActive) return;
        newPosition = new Vector3(followObject.transform.position.x, followObject.transform.position.y, -10f);
        mainCam.transform.position = newPosition;
    }

    private void DisableCameraFollow(){ cameraActive = false; }

}
