using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private Camera sceneCamera;
    [SerializeField]
    private LayerMask placementLayerMask;
    
    private Vector3 lastPosition;

    public Vector3 GetSelectedMapPosition() {
        Vector3 mousePos = Input.mousePosition;
        Ray ray = sceneCamera.ScreenPointToRay(mousePos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100, placementLayerMask)) {
            lastPosition = hit.point;
        }

        return lastPosition;
    }
}
