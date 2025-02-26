using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Camera cam;
    
    [SerializeField]
    private float minZoomDistance = 1.5f;
    private float maxZoomDistance = 8f;

    public void RotateCamera(float degree) {
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + degree, transform.rotation.eulerAngles.z);
    }

    public void ZoomCamera() {
        cam.orthographicSize = Mathf.Clamp(5.0f, minZoomDistance, maxZoomDistance);
    }
}
