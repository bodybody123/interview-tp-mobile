using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private Camera sceneCamera;
    [SerializeField]
    private LayerMask placementLayerMask;
    private Vector3 lastPosition;

    private PlayerInput playerInput;
    void Awake() {
        playerInput = GetComponent<PlayerInput>();
    }

    public Vector3 GetSelectedMapPosition() {
        Vector3 pos = playerInput.actions["Position"].ReadValue<Vector2>();
        Vector3 mousePos = Input.mousePosition;
        Ray ray = sceneCamera.ScreenPointToRay(pos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100, placementLayerMask)) {
            lastPosition = hit.point;
        }

        return lastPosition;
    }
}
