using CustomCamera;
using UnityEngine;

public class CameraManipulator: MonoBehaviour
{
    [SerializeField] private AtlasCamera camera;
    [SerializeField] private bool returnCameraToDefaults;
    [SerializeField] private AtlasCamera.CameraPositionStats replacementPositionStats;
    [SerializeField] private AtlasCamera.CameraBehaviors replacementCameraBehaviors;

    private void Start()
    {
        camera = Camera.main.GetComponent<AtlasCamera>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(returnCameraToDefaults)
        {
            camera.ReturnToDefaults();
            return;
        }

        if(collision.GetComponent<Player>() != null)
        {
            camera.CurrentCameraBehaviors= replacementCameraBehaviors;
            camera.CurrentCameraPositionStats = replacementPositionStats;
        }
    }
}